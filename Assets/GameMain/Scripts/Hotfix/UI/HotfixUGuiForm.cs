using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Trinity;
namespace Trinity.Hotfix
{
    /// <summary>
    /// 热更新层UGUI界面
    /// </summary>
    public class HotfixUGuiForm
    {
        /// <summary>
        /// 主工程的界面逻辑脚本
        /// </summary>
        protected Trinity.HotfixUGuiForm UIFormLogic
        {
            get;
            private set;
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        public virtual void OnInit(Trinity.HotfixUGuiForm uiFormLogic,object userdata)
        {
            UIFormLogic = uiFormLogic;
        }

        /// <summary>
        /// 界面打开
        /// </summary>
        public virtual void OnOpen(object userdata)
        {

        }

        /// <summary>
        /// 界面关闭
        /// </summary>
        public virtual void OnClose(object userdata)
        {

        }

        /// <summary>
        /// 界面暂停
        /// </summary>
        public virtual void OnPause()
        {

        }

        /// <summary>
        /// 界面暂停恢复
        /// </summary>
        public virtual void OnResume(object userdata)
        {

        }

        /// <summary>
        /// 界面遮挡
        /// </summary>
        public virtual void OnCover(object userdata)
        {

        }

        /// <summary>
        /// 界面遮挡恢复
        /// </summary>
        public virtual void OnReveal()
        {

        }

        /// <summary>
        /// 界面激活
        /// </summary>
        public virtual void OnRefocus(object userData)
        {

        }

        /// <summary>
        /// 界面轮询
        /// </summary>
        public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {

        }

        /// <summary>
        /// 界面深度改变
        /// </summary>
        public virtual void OnDepthChanged(int uiGroupDepth, int depthInUIGroup)
        {

        }
    }
}

