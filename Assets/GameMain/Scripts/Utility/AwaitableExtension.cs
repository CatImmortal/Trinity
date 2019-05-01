using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework.Event;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
namespace Trinity
{

    /// <summary>
    /// 可等待扩展
    /// </summary>
    public static class AwaitableExtension
    {
        private static TaskCompletionSource<UIForm> m_UIFormTcs;
        private static TaskCompletionSource<object> m_AssetTcs;
        private static TaskCompletionSource<EntityLogic> m_EntityTcs;

        private static int? m_UIFormSerialId;
        private static Type m_LogicType;

        /// <summary>
        /// 打开界面（可等待）
        /// </summary>
        public static Task<UIForm> AwaitOpenUIForm(this UIComponent uiComponent,UIFormId uiFormId, object userData = null)
        {
            m_UIFormTcs = new TaskCompletionSource<UIForm>();
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            m_UIFormSerialId = GameEntry.UI.OpenUIForm(uiFormId, userData);
            return m_UIFormTcs.Task;
        }

        /// <summary>
        /// 显示实体（可等待）
        /// </summary>
        /// <returns></returns>
        public static Task<EntityLogic> AwaitShowEntity(this EntityComponent entityComponent, Type logicType, int priority, EntityData data)
        {
            m_EntityTcs = new TaskCompletionSource<EntityLogic>();
            m_LogicType = logicType;
            entityComponent.ShowEntity(logicType, priority, data);
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            return m_EntityTcs.Task;
        }

        /// <summary>
        /// 加载资源（可等待）
        /// </summary>
        private static Task<T> AwaitLoadAsset<T>(this ResourceComponent resourceComponent,string assetName) where T:class
        {
            m_AssetTcs = new TaskCompletionSource<object>();
            GameEntry.Resource.LoadAsset(assetName,new LoadAssetCallbacks((tempAssetName, asset, duration, userData) => {
                m_AssetTcs.SetResult(asset);
            }));
            return m_AssetTcs.Task as Task<T>;
        }


        private static void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = e as OpenUIFormSuccessEventArgs;

            if (m_UIFormSerialId.HasValue && ne.UIForm.SerialId == m_UIFormSerialId.Value)
            {
                GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
                m_UIFormTcs.SetResult(ne.UIForm);
            }
        }

        private static void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = e as ShowEntitySuccessEventArgs;

            if (ne.EntityLogicType == m_LogicType)
            {
                m_EntityTcs.SetResult(ne.Entity.Logic);
            }
        }

    }
}

