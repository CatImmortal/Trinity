using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using GameFramework;
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
        private static TaskCompletionSource<EntityLogic> m_EntityTcs;
        private static TaskCompletionSource<byte[]> m_WebRequestTcs;

        private static int? m_UIFormSerialId;
        private static int m_EntitySerialId;
        private static int m_WebRequestSerialId;

        /// <summary>
        /// 打开界面（可等待）
        /// </summary>
        public static Task<UIForm> AwaitOpenUIForm(this UIComponent uiComponent,UIFormId uiFormId, object userData = null)
        {
            m_UIFormTcs = new TaskCompletionSource<UIForm>();
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
            m_UIFormSerialId = GameEntry.UI.OpenUIForm(uiFormId, userData);
            return m_UIFormTcs.Task;
        }

        /// <summary>
        /// 显示实体（可等待）
        /// </summary>
        public static Task<EntityLogic> AwaitShowEntity(this EntityComponent entityComponent, Type logicType, int priority, EntityData data)
        {
            m_EntityTcs = new TaskCompletionSource<EntityLogic>();
            m_EntitySerialId = data.Id;
            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            entityComponent.ShowEntity(logicType, priority, data);
            return m_EntityTcs.Task;
        }

        /// <summary>
        /// 增加Web请求任务（可等待）
        /// </summary>
        public static Task<byte[]> AwaitAddWebRequest(this WebRequestComponent webRequestComponent, string webRequestUri,byte[] postData = null)
        {
            m_WebRequestTcs = new TaskCompletionSource<byte[]>();
            m_WebRequestSerialId = webRequestComponent.AddWebRequest(webRequestUri, postData);
            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
            return m_WebRequestTcs.Task;
        }

        /// <summary>
        /// 加载资源（可等待）
        /// </summary>
        public static Task<T> AwaitLoadAsset<T>(this ResourceComponent resourceComponent, string assetName) where T : class
        {
            TaskCompletionSource<T> LoadAssetTcs = new TaskCompletionSource<T>();
            GameEntry.Resource.LoadAsset(assetName, new LoadAssetCallbacks(
                (tempAssetName, asset, duration, userData) => {
                LoadAssetTcs.SetResult(asset as T);
            }, 
                (tempAssetName, status, errorMessage, userData) => {
                LoadAssetTcs.SetException(new GameFrameworkException(errorMessage));
            }

            ));
            return LoadAssetTcs.Task;
        }

        private static void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e ;

            if (m_UIFormSerialId.HasValue && ne.UIForm.SerialId == m_UIFormSerialId.Value)
            {
                GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
                GameEntry.Event.Unsubscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
                m_UIFormTcs.SetResult(ne.UIForm);
            }
        }

        private static void OnOpenUIFormFailure(object sender, GameEventArgs e)
        {
            OpenUIFormFailureEventArgs ne = (OpenUIFormFailureEventArgs)e;
            if (m_UIFormSerialId.HasValue && ne.SerialId == m_UIFormSerialId.Value)
            {
                GameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
                GameEntry.Event.Unsubscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
                m_UIFormTcs.SetException(new GameFrameworkException(ne.ErrorMessage));
            }
        }

        private static void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            EntityData data = ne.UserData as EntityData;
            if (data.Id == m_EntitySerialId)
            {
                GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
                m_EntityTcs.SetResult(ne.Entity.Logic);
            }
        }

        private static void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            if (ne.EntityId == m_EntitySerialId)
            {
                GameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
                GameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
                m_EntityTcs.SetException(new GameFrameworkException(ne.ErrorMessage));
            }
        }

        private static void OnWebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;
            if (ne.SerialId == m_WebRequestSerialId)
            {
                GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
                GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
                m_WebRequestTcs.SetResult(ne.GetWebResponseBytes());
            }
        }

        private static void OnWebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;
            if (ne.SerialId == m_WebRequestSerialId)
            {
                GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
                GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);
                m_WebRequestTcs.SetException(new GameFrameworkException(ne.ErrorMessage));
            }
        }
    }
}

