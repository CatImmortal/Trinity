using UnityEditor;
using UnityEngine;
using System.IO;
using System;

namespace Trinity.Editor
{
    public class BuildHotfixEditor
    {
        private const string ScriptAssembliesDir = "Library/ScriptAssemblies";
        private const string CodeDir = "Assets/GameMain/HotfixDLL/";
        private const string HotfixDll = "Trinity.Hotfix.dll";
        private const string HotfixPdb = "Trinity.Hotfix.pdb";

        [MenuItem("Trinity/构建热更新DLL")]
        public static void BuildHotfixDLL()
        {
            //// Copy最新的pdb文件
            //string[] pdbDirs =
            //{
            //    "Temp/UnityVS_bin/Debug",
            //    "Temp/UnityVS_bin/Release",
            //    "Temp/Debug",
            //    "Temp/Release",
            //    "Temp/bin/Debug",
            //    "Temp/bin/Release"
            //};
            //DateTime dateTime = DateTime.MinValue;
            //string newestPdb = "";
            //string newestDll = "";

            //foreach (string pdbDir in pdbDirs)
            //{
            //    string pdbPath = Path.Combine(pdbDir, HotfixPdb);
            //    if (!File.Exists(pdbPath))
            //    {
            //        continue;
            //    }
            //    FileInfo fi = new FileInfo(pdbPath);
            //    DateTime lastWriteTimeUtc = fi.LastWriteTimeUtc;
            //    if (lastWriteTimeUtc > dateTime)
            //    {
            //        newestPdb = pdbPath;
            //        newestDll = Path.Combine(pdbDir, HotfixDll);
            //        dateTime = lastWriteTimeUtc;
            //    }
            //}

            //if (newestPdb != "")
            //{
            //    File.Copy(Path.Combine(newestDll), Path.Combine(CodeDir, "Hotfix.dll.bytes"), true);
            //    File.Copy(Path.Combine(newestPdb), Path.Combine(CodeDir, "Hotfix.pdb.bytes"), true);
            //    Debug.Log($"复制Hotfix.dll跟Hotfix.pdb到{CodeDir}完成");
            //}
            //else
            //{
            //    Debug.LogError("Hotfix.dll与Hotfix.pdb不存在目录下");
            //}

            //————————————————————————————————
            File.Copy(Path.Combine(ScriptAssembliesDir, HotfixDll), Path.Combine(CodeDir, "Hotfix.dll.bytes"), true);
            File.Copy(Path.Combine(ScriptAssembliesDir, HotfixPdb), Path.Combine(CodeDir, "Hotfix.pdb.bytes"), true);
            Debug.Log("复制Hotfix.dll, Hotfix.pdb到Asset/GameMain/HotfixDLL完成");
            AssetDatabase.Refresh();

        }


    }
}

