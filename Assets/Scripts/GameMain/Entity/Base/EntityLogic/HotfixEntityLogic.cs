using GameFramework;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Trinity
{
    /// <summary>
    /// 热更新层实体逻辑
    /// </summary>
    public class HotfixEntityLogic : EntityLogic
    {
        [SerializeField]
        private HotfixEntityData m_Data;

        /// <summary>
        /// 对应的热更新层实体逻辑类名
        /// </summary>
        [SerializeField]
        private string m_HotfixEntityLogicName;

        /// <summary>
        /// 对应的热更新层实体逻辑类实例
        /// </summary>
        private object m_HotfixInstance;

        //热更新层的方法缓存
        private ILInstanceMethod m_OnShow;
        private ILInstanceMethod m_OnHide;
        private ILInstanceMethod m_OnAttached;
        private ILInstanceMethod m_OnDetached;
        private ILInstanceMethod m_OnAttachTo;
        private ILInstanceMethod m_OnDetachFrom;
        private IMethod m_OnUpdate;


        protected override void OnInit(object userData)
        {
            base.OnInit(userData);

            m_Data = (HotfixEntityData)userData;
            m_HotfixEntityLogicName = m_Data.HotfixEntityName;

            string hotfixEntityLogicFullName = Utility.Text.Format("{0}.{1}", "Trinity.Hotfix", m_HotfixEntityLogicName);

            //获取热更新层的实例
            IType type = GameEntry.ILRuntime.AppDomain.LoadedTypes[hotfixEntityLogicFullName];
            m_HotfixInstance = ((ILType)type).Instantiate();

            //获取热更新层的方法并缓存
            m_OnShow = new ILInstanceMethod(m_HotfixInstance, hotfixEntityLogicFullName, "OnShow", 1);
            m_OnHide = new ILInstanceMethod(m_HotfixInstance, hotfixEntityLogicFullName, "OnHide", 1);
            m_OnAttached = new ILInstanceMethod(m_HotfixInstance, hotfixEntityLogicFullName, "OnAttached", 3);
            m_OnDetached = new ILInstanceMethod(m_HotfixInstance, hotfixEntityLogicFullName, "OnDetached", 2);
            m_OnAttachTo = new ILInstanceMethod(m_HotfixInstance, hotfixEntityLogicFullName, "OnAttachTo", 3);
            m_OnDetachFrom = new ILInstanceMethod(m_HotfixInstance, hotfixEntityLogicFullName, "OnDetachFrom", 2);
            m_OnUpdate = type.GetMethod("OnUpdate", 2);

            //调用热更新层的OnInit
            GameEntry.ILRuntime.AppDomain.Invoke(hotfixEntityLogicFullName, "OnInit", m_HotfixInstance, this, m_Data.UserData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            m_Data = (HotfixEntityData)userData;
            m_OnShow.Invoke(m_Data.UserData);
        }

        protected override void OnHide(object userData)
        {
            base.OnHide(userData);

            m_OnHide.Invoke(userData);

            //归还实体数据引用
            ReferencePool.Release(m_Data);
        }

        protected override void OnAttached(UnityGameFramework.Runtime.EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);

            m_OnAttached.Invoke(childEntity, parentTransform, userData);
        }

        protected override void OnDetached(UnityGameFramework.Runtime.EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);

            m_OnDetached.Invoke(childEntity, userData);
        }

        protected override void OnAttachTo(UnityGameFramework.Runtime.EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);

            m_OnAttachTo.Invoke(parentEntity, parentTransform, userData);
        }

        protected override void OnDetachFrom(UnityGameFramework.Runtime.EntityLogic parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);

            m_OnDetachFrom.Invoke(parentEntity, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            using (var ctx = GameEntry.ILRuntime.AppDomain.BeginInvoke(m_OnUpdate))
            {
                ctx.PushObject(m_HotfixInstance);
                ctx.PushFloat(elapseSeconds);
                ctx.PushFloat(realElapseSeconds);
                ctx.Invoke();
            }
        }
    }
}

