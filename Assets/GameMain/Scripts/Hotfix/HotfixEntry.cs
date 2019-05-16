using UnityEngine;
using GameFramework.Event;
using UnityGameFramework.Runtime;

namespace Trinity.Hotfix
{
    /// <summary>
    /// 热更新层入口
    /// </summary>
    public class HotfixEntry
    {
        private FsmManager m_FsmManager;
        private ProcedureManager m_ProcedureManager;

        public void Start()
        {
            Log.Info("热更新层启动!");
            m_FsmManager = new FsmManager();
            m_ProcedureManager = new ProcedureManager();

            //初始化流程管理器并开始流程
            m_ProcedureManager.Initialize(m_FsmManager
                , new ProcedureHotfixEntry()
                , new ProcedureChangeScene()
                , new ProcedureHotfixTest()
                );

            m_ProcedureManager.StartProcedure<ProcedureHotfixEntry>();

            //初始化ET网络
            ETNetwork.Init();
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            m_FsmManager.Update(elapseSeconds, realElapseSeconds);
        }

        public void ShutDown()
        {
            m_ProcedureManager.Shutdown();
            m_FsmManager.Shutdown();
            ETNetwork.Shutdown();
        }
    }
}

