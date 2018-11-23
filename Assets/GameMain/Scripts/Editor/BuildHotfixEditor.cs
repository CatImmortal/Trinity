using UnityEditor;
using UnityEngine;
using System.IO;

namespace Trinity.Editor
{
    public class BuildHotfixEditor
    {

        private const string DebugDir = "Temp/bin/Debug/";
        private const string CodeDir = "Assets/GameMain/HotfixDLL/";
        private const string HotfixDll = "Trinity.Hotfix.dll";
        private const string HotfixPdb = "Trinity.Hotfix.pdb";

        [MenuItem("Trinity/构建热更新DLL")]
        public static void BuildHotfixDLL()
        {
            if (File.Exists(Path.Combine(DebugDir, HotfixPdb)))
            {
                File.Copy(Path.Combine(DebugDir, HotfixPdb), Path.Combine(CodeDir, "Hotfix.pdb.bytes"), true);
                Debug.Log("复制Hotfix.pdb到" + CodeDir + "完成");
            }
            else
            {
                Debug.LogError("Hotfix.pdb不存在" + DebugDir + "目录下");
            }


            if (File.Exists(Path.Combine(DebugDir, HotfixDll)))
            {
                File.Copy(Path.Combine(DebugDir, HotfixDll), Path.Combine(CodeDir, "Hotfix.dll.bytes"), true);
                Debug.Log("复制Hotfix.dll到" + CodeDir + "完成");
            }
            else
            {
                Debug.LogError("Hotfix.dll不存在" + DebugDir + "目录下");
            }


            AssetDatabase.Refresh();

        }


    }
}

