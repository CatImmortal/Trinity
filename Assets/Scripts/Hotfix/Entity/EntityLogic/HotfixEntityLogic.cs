using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Trinity.Hotfix
{
    /// <summary>
    /// 热更新层实体
    /// </summary>
    public class HotfixEntityLogic
    {

        /// <summary>
        /// 主工程的实体逻辑脚本
        /// </summary>
        protected Trinity.HotfixEntityLogic EntityLogic
        {
            get;
            private set;
        }

        /// <summary>
        /// 实体初始化
        /// </summary>
        public virtual void OnInit(Trinity.HotfixEntityLogic entityLogic, object userData)
        {
            EntityLogic = entityLogic;
        }

        /// <summary>
        /// 实体显示
        /// </summary>
        public virtual void OnShow(object userData)
        {

        }

        /// <summary>
        /// 实体隐藏
        /// </summary>
        /// <param name="userData"></param>
        public virtual void OnHide(object userData)
        {

        }

        /// <summary>
        /// 实体附加子实体
        /// </summary>
        public virtual void OnAttached(UnityGameFramework.Runtime.EntityLogic childEntity, Transform parentTransform, object userData)
        {

        }

        /// <summary>
        /// 实体解除子实体
        /// </summary>
        public virtual void OnDetached(UnityGameFramework.Runtime.EntityLogic childEntity, object userData)
        {

        }

        /// <summary>
        /// 实体附加子实体
        /// </summary>
        public virtual void OnAttachTo(UnityGameFramework.Runtime.EntityLogic parentEntity, Transform parentTransform, object userData)
        {

        }

        /// <summary>
        /// 实体解除子实体
        /// </summary>
        public virtual void OnDetachFrom(UnityGameFramework.Runtime.EntityLogic parentEntity, object userData)
        {

        }

        /// <summary>
        /// 实体轮询
        /// </summary>
        public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {

        }
    }
}

