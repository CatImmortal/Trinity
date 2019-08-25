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

        private static TaskCompletionSource<UIForm> s_UIFormTcs;
        private static TaskCompletionSource<Entity> s_EntityTcs;
        private static TaskCompletionSource<bool> s_SceneTcs;
        private static TaskCompletionSource<byte[]> s_WebRequestTcs;
        private static TaskCompletionSource<bool> s_DownloadTcs;
        private static TaskCompletionSource<object> s_LoadAssetTcs;


        private static int? s_UIFormSerialId;
        private static int s_EntitySerialId;
        private static string s_LoadSceneAssetName;
        private static int s_WebRequestSerialId;
        private static int s_DownloadSerialId;
        private static LoadAssetCallbacks s_LoadAssetCallbacks;

        static AwaitableExtension()
        {
            GameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            GameEntry.Event.Subscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);

            GameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            GameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);

            GameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
            GameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);

            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

            GameEntry.Event.Subscribe(DownloadSuccessEventArgs.EventId, OnDownloadSuccess);
            GameEntry.Event.Subscribe(DownloadFailureEventArgs.EventId, OnDownloadFailure);

            s_UIFormSerialId = null;
            s_EntitySerialId = int.MinValue;
            s_LoadSceneAssetName = null;
            s_WebRequestSerialId = int.MinValue;
            s_DownloadSerialId = int.MinValue;
            s_LoadAssetCallbacks = new LoadAssetCallbacks(OnLoadAssetSuccess, OnLoadAssetFailure);
        }

        /// <summary>
        /// 打开界面（可等待）
        /// </summary>
        public static Task<UIForm> AwaitOpenUIForm(this UIComponent self, UIFormId uiFormId, object userData = null)
        {
            s_UIFormTcs = new TaskCompletionSource<UIForm>();
            s_UIFormSerialId = self.OpenUIForm(uiFormId, userData);
            return s_UIFormTcs.Task;
        }

        /// <summary>
        /// 打开界面（可等待）
        /// </summary>
        public static Task<UIForm> AwaitOpenUIForm(this UIComponent self, int uiFormId, object userData = null)
        {
            s_UIFormTcs = new TaskCompletionSource<UIForm>();
            s_UIFormSerialId = self.OpenUIForm(uiFormId, userData);
            return s_UIFormTcs.Task;
        }

        private static void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (s_UIFormSerialId.HasValue && ne.UIForm.SerialId == s_UIFormSerialId.Value)
            {
                s_UIFormTcs.SetResult(ne.UIForm);
                s_UIFormTcs = null;
            }
        }

        private static void OnOpenUIFormFailure(object sender, GameEventArgs e)
        {
            OpenUIFormFailureEventArgs ne = (OpenUIFormFailureEventArgs)e;
            if (s_UIFormSerialId.HasValue && ne.SerialId == s_UIFormSerialId.Value)
            {
                s_UIFormTcs.SetException(new GameFrameworkException(ne.ErrorMessage));
                s_UIFormTcs = null;
            }
        }

        /// <summary>
        /// 显示实体（可等待）
        /// </summary>
        public static Task<Entity> AwaitShowEntity(this EntityComponent self, Type logicType, int priority, EntityData data)
        {
            s_EntityTcs = new TaskCompletionSource<Entity>();
            s_EntitySerialId = data.Id;
            self.ShowEntity(logicType, priority, data);
            return s_EntityTcs.Task;
        }

        /// <summary>
        /// 显示热更新层实体（可等待）
        /// </summary>
        public static Task<Entity> AwaitShowHotfixEntity(this EntityComponent self, int priority, HotfixEntityData data)
        {
            s_EntityTcs = new TaskCompletionSource<Entity>();
            s_EntitySerialId = data.Id;
            self.ShowHotfixEntity(priority, data);
            return s_EntityTcs.Task;
        }

        private static void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            EntityData data = (EntityData)ne.UserData;
            if (data.Id == s_EntitySerialId)
            {
                s_EntityTcs.SetResult(ne.Entity);
                s_EntityTcs = null;
            }
        }

        private static void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            if (ne.EntityId == s_EntitySerialId)
            {
                s_EntityTcs.SetException(new GameFrameworkException(ne.ErrorMessage));
                s_EntityTcs = null;
            }
        }


        /// <summary>
        /// 加载场景（可等待）
        /// </summary>
        public static Task<bool> AwaitLoadScene(this SceneComponent self, string sceneAssetName)
        {
            s_SceneTcs = new TaskCompletionSource<bool>();
            s_LoadSceneAssetName = sceneAssetName;
            self.LoadScene(sceneAssetName);
            return s_SceneTcs.Task;
        }
        private static void OnLoadSceneSuccess(object sender, GameEventArgs e)
        {
            LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
            if (ne.SceneAssetName == s_LoadSceneAssetName)
            {
                s_SceneTcs.SetResult(true);
                s_SceneTcs = null;
            }
        }

        private static void OnLoadSceneFailure(object sender, GameEventArgs e)
        {
            LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
            if (ne.SceneAssetName == s_LoadSceneAssetName)
            {
                s_SceneTcs.SetException(new GameFrameworkException(ne.ErrorMessage));
                s_SceneTcs = null;
            }
        }


        /// <summary>
        /// 加载多个资源（可等待）
        /// </summary>
        public static async Task<object[]> AwaitLoadAssets(this ResourceComponent self, string[] assetNames)
        {
            object[] assets = new object[assetNames.Length]; ;

            for (int i = 0; i < assetNames.Length; i++)
            {
                assets[i] = await self.AwaitLoadAsset<object>(assetNames[i]);
            }

            return assets;
        }

        /// <summary>
        /// 加载资源（可等待）
        /// </summary>
        public static async Task<T> AwaitLoadAsset<T>(this ResourceComponent self, string assetName)
        {
            object asset = await self.AwaitInternalLoadAsset<T>(assetName);
            return (T)asset;
        }

        private static Task<object> AwaitInternalLoadAsset<T>(this ResourceComponent self, string assetName)
        {
            s_LoadAssetTcs = new TaskCompletionSource<object>();

            self.LoadAsset(assetName, typeof(T), s_LoadAssetCallbacks);

            return s_LoadAssetTcs.Task;
        }

        private static void OnLoadAssetSuccess(string assetName, object asset, float duration, object userData)
        {
            s_LoadAssetTcs.SetResult(asset);
        }

        private static void OnLoadAssetFailure(string assetName, LoadResourceStatus status, string errorMessage, object userData)
        {
            s_LoadAssetTcs.SetException(new GameFrameworkException(errorMessage));
        }

        /// <summary>
        /// 增加Web请求任务（可等待）
        /// </summary>
        public static Task<byte[]> AwaitAddWebRequest(this WebRequestComponent self, string webRequestUri, byte[] postData = null)
        {
            s_WebRequestTcs = new TaskCompletionSource<byte[]>();
            s_WebRequestSerialId = self.AddWebRequest(webRequestUri, postData);
            return s_WebRequestTcs.Task;
        }

        private static void OnWebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;
            if (ne.SerialId == s_WebRequestSerialId)
            {
                s_WebRequestTcs.SetResult(ne.GetWebResponseBytes());
                s_WebRequestTcs = null;
            }
        }

        private static void OnWebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;
            if (ne.SerialId == s_WebRequestSerialId)
            {
                s_WebRequestTcs.SetException(new GameFrameworkException(ne.ErrorMessage));
                s_WebRequestTcs = null;
            }
        }

        /// <summary>
        /// 增加下载任务（可等待)
        /// </summary>
        public static Task<bool> AwaitAddDownload(this DownloadComponent self, string downloadPath, string downloadUri)
        {
            s_DownloadTcs = new TaskCompletionSource<bool>();
            s_DownloadSerialId = self.AddDownload(downloadPath, downloadUri);
            return s_DownloadTcs.Task;
        }

        private static void OnDownloadSuccess(object sender, GameEventArgs e)
        {
            DownloadSuccessEventArgs ne = (DownloadSuccessEventArgs)e;
            if (ne.SerialId == s_DownloadSerialId)
            {
                s_DownloadTcs.SetResult(true);
                s_DownloadTcs = null;
            }
        }
        private static void OnDownloadFailure(object sender, GameEventArgs e)
        {
            DownloadFailureEventArgs ne = (DownloadFailureEventArgs)e;
            if (ne.SerialId == s_DownloadSerialId)
            {
                s_DownloadTcs.SetException(new GameFrameworkException(ne.ErrorMessage));
                s_DownloadTcs = null;
            }
        }

    }
}

