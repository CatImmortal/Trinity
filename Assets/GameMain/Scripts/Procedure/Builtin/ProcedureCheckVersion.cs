using System;
using GameFramework;
using GameFramework.Event;
using GameFramework.Procedure;
using GameFramework.Resource;
using UnityEngine;
using UnityGameFramework.Runtime;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using Version = GameFramework.Version;

namespace Trinity
{
    public class ProcedureCheckVersion : ProcedureBase
    {
        private bool m_InitResourcesComplete = false;

        private UpdateVersionListCallbacks m_UpdateVersionListCallbacks;

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);

            m_InitResourcesComplete = false;
            m_UpdateVersionListCallbacks = new UpdateVersionListCallbacks(OnUpdateVersionListSuccess, OnUpdateVersionListFailure);

            GameEntry.Event.Subscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Subscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

            //可更新模式检查版本信息，否则直接初始化资源，
            if (GameEntry.Resource.ResourceMode == ResourceMode.Updatable)
            {
                RequestVersion();
            }
            else
            {
                GameEntry.Resource.InitResources(OnInitResourcesComplete);
            }
        }

      

        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            GameEntry.Event.Unsubscribe(WebRequestSuccessEventArgs.EventId, OnWebRequestSuccess);
            GameEntry.Event.Unsubscribe(WebRequestFailureEventArgs.EventId, OnWebRequestFailure);

            base.OnLeave(procedureOwner, isShutdown);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (!m_InitResourcesComplete)
            {
                return;
            }

            ChangeState<ProcedurePreload>(procedureOwner);
        }

        private void RequestVersion()
        {
            string deviceId = SystemInfo.deviceUniqueIdentifier;
            string deviceName = SystemInfo.deviceName;
            string deviceModel = SystemInfo.deviceModel;
            string processorType = SystemInfo.processorType;
            string processorCount = SystemInfo.processorCount.ToString();
            string memorySize = SystemInfo.systemMemorySize.ToString();
            string operatingSystem = SystemInfo.operatingSystem;
            string iOSGeneration = string.Empty;
            string iOSSystemVersion = string.Empty;
            string iOSVendorIdentifier = string.Empty;
#if UNITY_IOS && !UNITY_EDITOR
            iOSGeneration = UnityEngine.iOS.Device.generation.ToString();
            iOSSystemVersion = UnityEngine.iOS.Device.systemVersion;
            iOSVendorIdentifier = UnityEngine.iOS.Device.vendorIdentifier ?? string.Empty;
#endif
            string gameVersion = Version.GameVersion;
            string platform = Application.platform.ToString();
            string language = GameEntry.Localization.Language.ToString();
            string unityVersion = Application.unityVersion;
            string installMode = Application.installMode.ToString();
            string sandboxType = Application.sandboxType.ToString();
            string screenWidth = Screen.width.ToString();
            string screenHeight = Screen.height.ToString();
            string screenDpi = Screen.dpi.ToString();
            string screenOrientation = Screen.orientation.ToString();
            string screenResolution = string.Format("{0} x {1} @ {2}Hz", Screen.currentResolution.width.ToString(), Screen.currentResolution.height.ToString(), Screen.currentResolution.refreshRate.ToString());
            string useWifi = (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork).ToString();

            WWWForm wwwForm = new WWWForm();
            wwwForm.AddField("DeviceId", WebUtility.EscapeString(deviceId));
            wwwForm.AddField("DeviceName", WebUtility.EscapeString(deviceName));
            wwwForm.AddField("DeviceModel", WebUtility.EscapeString(deviceModel));
            wwwForm.AddField("ProcessorType", WebUtility.EscapeString(processorType));
            wwwForm.AddField("ProcessorCount", WebUtility.EscapeString(processorCount));
            wwwForm.AddField("MemorySize", WebUtility.EscapeString(memorySize));
            wwwForm.AddField("OperatingSystem", WebUtility.EscapeString(operatingSystem));
            wwwForm.AddField("IOSGeneration", WebUtility.EscapeString(iOSGeneration));
            wwwForm.AddField("IOSSystemVersion", WebUtility.EscapeString(iOSSystemVersion));
            wwwForm.AddField("IOSVendorIdentifier", WebUtility.EscapeString(iOSVendorIdentifier));
            wwwForm.AddField("GameVersion", WebUtility.EscapeString(gameVersion));
            wwwForm.AddField("Platform", WebUtility.EscapeString(platform));
            wwwForm.AddField("Language", WebUtility.EscapeString(language));
            wwwForm.AddField("UnityVersion", WebUtility.EscapeString(unityVersion));
            wwwForm.AddField("InstallMode", WebUtility.EscapeString(installMode));
            wwwForm.AddField("SandboxType", WebUtility.EscapeString(sandboxType));
            wwwForm.AddField("ScreenWidth", WebUtility.EscapeString(screenWidth));
            wwwForm.AddField("ScreenHeight", WebUtility.EscapeString(screenHeight));
            wwwForm.AddField("ScreenDPI", WebUtility.EscapeString(screenDpi));
            wwwForm.AddField("ScreenOrientation", WebUtility.EscapeString(screenOrientation));
            wwwForm.AddField("ScreenResolution", WebUtility.EscapeString(screenResolution));
            wwwForm.AddField("UseWifi", WebUtility.EscapeString(useWifi));

            //发送web请求，获取服务器上的版本信息（version.txt）
            GameEntry.WebRequest.AddWebRequest(GameEntry.BuiltinData.BuildInfo.CheckVersionUrl, wwwForm, this);
        }

        private void OnWebRequestSuccess(object sender, GameEventArgs e)
        {
            WebRequestSuccessEventArgs ne = (WebRequestSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            //解析服务器发来的版本信息
            string responseJson = Utility.Converter.GetString(ne.GetWebResponseBytes());
            VersionInfo versionInfo = Utility.Json.ToObject<VersionInfo>(responseJson);
            if (versionInfo == null)
            {
                Log.Error("Parse VersionInfo failure.");
                return;
            }

            Log.Info("Latest game version is '{0}', local game version is '{1}'.", versionInfo.LatestGameVersion, Version.GameVersion);

            if (versionInfo.GameUpdate)
            {
                //是否需要更新版本资源列表
                CheckVersionListResult result = GameEntry.Resource.CheckVersionList(versionInfo.InternalResourceVersion);
                if (result == CheckVersionListResult.NeedUpdate)
                {
                    Log.Info("最新内部资源版本号为{0}，需要更新版本资源列表", versionInfo.InternalResourceVersion);

                    //更新版本资源列表
                    GameEntry.Resource.UpdatePrefixUri = versionInfo.GameUpdateUrl;
                    GameEntry.Resource.UpdateVersionList(versionInfo.VersionListLength, versionInfo.VersionListHashCode, versionInfo.VersionListZipLength, versionInfo.VersionListZipHashCode, m_UpdateVersionListCallbacks);
                }
                else
                {
                    //检查资源
                    Log.Info("最新内部资源版本号为{0}，不需要更新版本资源列表，开始检查需要更新的资源", versionInfo.InternalResourceVersion);
                    GameEntry.Resource.CheckResources(OnCheckResourcesComplete);
                }
                
            }

            
        }

        private void OnWebRequestFailure(object sender, GameEventArgs e)
        {
            WebRequestFailureEventArgs ne = (WebRequestFailureEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }

            Log.Warning("Check version failure.");
        }

        private void OnInitResourcesComplete()
        {
            m_InitResourcesComplete = true;

            Log.Info("Init resources complete.");
        }

        private void OnUpdateVersionListSuccess(string downloadPath, string downloadUri)
        {
            Log.Info("更新版本资源列表成功，开始检查需要更新的资源");
            GameEntry.Resource.CheckResources(OnCheckResourcesComplete);
        }

        private void OnUpdateVersionListFailure(string downloadUri, string errorMessage)
        {
            Log.Fatal("更新版本资源列表失败，url:{0},errorMessage{1}", downloadUri, errorMessage);
        }


        private void OnCheckResourcesComplete(bool needUpdateResources, int removedCount, int updateCount, int updateTotalLength, int updateTotalZipLength)
        {
            if (needUpdateResources && updateCount > 0)
            {
                Log.Info("需要更新资源，更新数量：{0},更新压缩包大小：{1}", updateCount, updateTotalZipLength);
                GameEntry.Resource.UpdateResources(UpdateResourcesComplete);
            }
            else
            {
                Log.Info("不需要更新资源");
                m_InitResourcesComplete = true;
            }
        }

        private void UpdateResourcesComplete()
        {
            Log.Info("资源更新完毕");

            m_InitResourcesComplete = true;
        }
    }
}
