﻿using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework;
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;

namespace Trinity
{
    /// <summary>
    /// 热更新层UGUI界面
    /// </summary>
    public class HotfixUGuiForm : UGuiForm
    {
        /// <summary>
        /// 对应的热更新层UGUI界面类名
        /// </summary>
        [SerializeField]
        private string m_HotfixUGuiFormName;

        //热更新层的方法缓存
        private ILInstanceMethod m_OnOpen;
        private ILInstanceMethod m_OnClose;
        private ILInstanceMethod m_OnPause;
        private ILInstanceMethod m_OnResume;
        private ILInstanceMethod m_OnCover;
        private ILInstanceMethod m_OnReveal;
        private ILInstanceMethod m_OnRefocus;
        private ILInstanceMethod m_OnUpdate;
        private ILInstanceMethod m_OnDepthChanged;

        [SerializeField]
        private bool m_CanUpdate;

        /// <summary>
        /// 是否调用Update方法
        /// </summary>
        public bool CanUpdate
        {
            get
            {
                return m_CanUpdate;
            }

            set
            {
                m_CanUpdate = value;
            }
        }

        /// <summary>
        /// 界面初始化（热更新层新增界面用）
        /// </summary>
        public void OnHotfixInit(string hotfixUGuiFormName,object userData = null)
        {
            if (string.IsNullOrEmpty(m_HotfixUGuiFormName))
            {
                m_HotfixUGuiFormName = hotfixUGuiFormName;
                OnInit(userData);
                OnOpen(userData);
                UIForm.Logic = this;
                GameEntry.UI.RefocusUIForm(UIForm);
            }
        }

        protected override void OnInit(object userData)
        {
            base.OnInit(userData);
            m_HotfixUGuiFormName = this.GetType().Name;
            string hotfixUGuiFormFullName = Utility.Text.Format("{0}.{1}", "Trinity.Hotfix", m_HotfixUGuiFormName);
            Debug.Log(hotfixUGuiFormFullName);
            //获取热更新层的实例
            IType type = GameEntry.ILRuntime.AppDomain.LoadedTypes[hotfixUGuiFormFullName];
            object hotfixInstance = ((ILType)type).Instantiate();

            //获取热更新层的方法并缓存
            m_OnOpen = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnOpen", 1);
            m_OnClose = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnClose", 1);
            m_OnPause = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnPause", 0);
            m_OnResume = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnResume", 0);
            m_OnCover = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnCover", 0);
            m_OnReveal = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnReveal", 0);
            m_OnRefocus = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnRefocus", 1);
            m_OnUpdate = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnUpdate", 2);
            m_OnDepthChanged = new ILInstanceMethod(hotfixInstance, hotfixUGuiFormFullName, "OnDepthChanged", 2);

            //调用热更新层的OnInit
            GameEntry.ILRuntime.AppDomain.Invoke(hotfixUGuiFormFullName, "OnInit", hotfixInstance, this,userData);

        }

        protected override void OnOpen(object userData)
        {
            base.OnOpen(userData);

            m_OnOpen?.Invoke(userData);
        }

        protected override void OnClose(object userData)
        {
            base.OnClose(userData);

            m_OnClose?.Invoke(userData);
        }

        protected override void OnPause()
        {
            base.OnPause();

            m_OnPause?.Invoke();
        }

        protected override void OnResume()
        {
            base.OnResume();

            m_OnResume?.Invoke();
        }

        protected override void OnCover()
        {
            base.OnCover();

            m_OnCover?.Invoke();
        }

        protected override void OnReveal()
        {
            base.OnReveal();

            m_OnReveal?.Invoke();
        }

        protected override void OnRefocus(object userData)
        {
            base.OnRefocus(userData);

            m_OnRefocus?.Invoke(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            if (!CanUpdate)
            {
                return;
            }

            base.OnUpdate(elapseSeconds, realElapseSeconds);

            m_OnUpdate?.Invoke(elapseSeconds, realElapseSeconds);

        }

        protected override void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {
            base.OnDepthChanged(uiGroupDepth, depthInUIGroup);

            m_OnDepthChanged?.Invoke(uiGroupDepth, depthInUIGroup);
        }
    }

}
