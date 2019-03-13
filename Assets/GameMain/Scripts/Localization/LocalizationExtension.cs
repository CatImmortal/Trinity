using GameFramework;
using UnityGameFramework.Runtime;

namespace Trinity
{
    public static class LocalizationExtension
    {
        public static void LoadDictionary(this LocalizationComponent localizationComponent, string dictionaryName, LoadType loadType, object userData = null)
        {
            if (string.IsNullOrEmpty(dictionaryName))
            {
                Log.Warning("Dictionary name is invalid.");
                return;
            }

            localizationComponent.LoadDictionary(dictionaryName, AssetUtility.GetDictionaryAsset(dictionaryName, loadType), loadType, Constant.AssetPriority.DictionaryAsset, userData);
        }
    }
}
