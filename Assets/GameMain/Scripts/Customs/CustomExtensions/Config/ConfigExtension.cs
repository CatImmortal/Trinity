using GameFramework;
using UnityGameFramework.Runtime;

namespace Trinity
{
    public static class ConfigExtension
    {
        public static void LoadConfig(this ConfigComponent configComponent, string configName, LoadType loadType, object userData = null)
        {
            if (string.IsNullOrEmpty(configName))
            {
                Log.Warning("Config name is invalid.");
                return;
            }

            configComponent.LoadConfig(configName, AssetUtility.GetConfigAsset(configName, loadType), loadType, Constant.AssetPriority.ConfigAsset, userData);
        }
    }
}
