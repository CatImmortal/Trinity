using GameFramework.Resource;
using ILRuntime.CLR.TypeSystem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityGameFramework.Runtime;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace Trinity
{
    /// <summary>
    /// ILRuntime组件
    /// </summary>
    public class ILRuntimeComponent : GameFrameworkComponent
    {
        /// <summary>
        /// ILRuntime入口对象
        /// </summary>
        public AppDomain AppDomain
        {
            get;
            private set;
        }

        /// <summary>
        /// 热更新DLL是否已加载完成
        /// </summary>
        public bool HotfixLoaded
        {
            get;
            private set;
        }

        /// <summary>
        /// 是否开启ILRuntime模式
        /// </summary>
        public bool IsILRuntimeMode;

        /// <summary>
        /// 资源加载回调方法集
        /// </summary>
        private LoadAssetCallbacks m_LoadAssetCallbacks;

        /// <summary>
        /// Hotfix.dll是否已加载
        /// </summary>
        private bool m_DLLLoaded = false;

        /// <summary>
        /// Hotfix.pdb是否已加载
        /// </summary>
        private bool m_PDBLoaded = false;

        /// <summary>
        /// 保存Hotfix.dll的字节数组
        /// </summary>
        private byte[] m_DLL;

        /// <summary>
        /// 保存Hotfix.pdb的字节数组
        /// </summary>
        private byte[] m_PDB;

        
        private ILInstanceMethod m_Update;
        private ILInstanceMethod m_ShutDown;

        private MemoryStream m_DLLStream;
        private MemoryStream m_PDBStream;

        private void Update()
        {
            m_Update?.Invoke(Time.deltaTime, Time.unscaledDeltaTime);
        }

        private void OnDestroy()
        {
            m_ShutDown?.Invoke();
        }

        /// <summary>
        /// 获取热更新层类的Type对象
        /// </summary>
        public Type GetHotfixType(string hotfixTypeFullName)
        {
            return AppDomain.LoadedTypes[hotfixTypeFullName].ReflectionType;
        }

        /// <summary>
        /// 获取所有热更新层类的Type对象
        /// </summary>
        /// <returns></returns>
        public List<Type> GetHotfixTypes()
        {
            return AppDomain.LoadedTypes.Values.Select(x => x.ReflectionType).ToList();
        }

        /// <summary>
        /// 加载热更新DLL
        /// </summary>
        public void LoadHotfixDLL()
        {
            HotfixLoaded = false;

            if (IsILRuntimeMode)
            {
                AppDomain = new AppDomain();
                ILRuntimeUtility.InitILRuntime(AppDomain);

                //启动调试服务器
                AppDomain.DebugService.StartDebugService(56000);
                Log.Info("启动了ILRuntime调试服务器");

                m_LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadHotfixDLLSuccess, OnLoadHotfixDLLFailure);
                
                GameEntry.Resource.LoadAsset(AssetUtility.GetHotfixDLLAsset("Hotfix.dll"), typeof(TextAsset), m_LoadAssetCallbacks,1);
                GameEntry.Resource.LoadAsset(AssetUtility.GetHotfixDLLAsset("Hotfix.pdb"), typeof(TextAsset), m_LoadAssetCallbacks,2);
            }

        }

        private void OnLoadHotfixDLLSuccess(string assetName, object asset, float duration, object userData)
        {

            if ((int)userData == 1)
            {
                Log.Info("Hotfix.dll加载成功");
                m_DLLLoaded = true;
                m_DLL = (asset as TextAsset).bytes;
            }
            else
            {
                Log.Info("Hotfix.pdb加载成功");
                m_PDBLoaded = true;
                m_PDB = (asset as TextAsset).bytes;
            }

            if (m_DLLLoaded && m_PDBLoaded)
            {
                HotfixLoaded = true;

                m_DLLLoaded = false;
                m_PDBLoaded = false;

                m_DLLStream = new MemoryStream(m_DLL);
                m_PDBStream = new MemoryStream(m_PDB);

                AppDomain.LoadAssembly(m_DLLStream, m_PDBStream, new Mono.Cecil.Pdb.PdbReaderProvider());

            }
        }

        private void OnLoadHotfixDLLFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            if ((int)userData == 1)
            {
                Log.Error("Hotfix.dll加载失败：{0}", errorMessage);
            }
            else
            {
                Log.Error("Hotfix.pdb加载失败：{0}", errorMessage);
            }
        }

        /// <summary>
        /// 开始执行热更新层代码
        /// </summary>
        public void HotfixStart()
        {
            //防止多次调用
            HotfixLoaded = false;

            string typeFullName = "Trinity.Hotfix.HotfixEntry";
            IType type = AppDomain.LoadedTypes[typeFullName];
            object hotfixInstance = ((ILType)type).Instantiate();

            AppDomain.Invoke(typeFullName, "Start", hotfixInstance, null);

            m_Update = new ILInstanceMethod(hotfixInstance, typeFullName, "Update", 2);
            m_ShutDown = new ILInstanceMethod(hotfixInstance, typeFullName, "ShutDown", 0);
        }


    }

}
