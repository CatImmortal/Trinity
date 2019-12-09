using UnityEditor;
using UnityEngine;
using System.IO;
using System;

namespace Trinity.Editor
{
    public static class BuildHotfixEditor
    {
        private const string ScriptAssembliesDir = "Library/ScriptAssemblies";
        private const string CodeDir = "Assets/Res/HotfixDLL/";
        private const string HotfixDll = "Trinity.Hotfix.dll";
        private const string HotfixPdb = "Trinity.Hotfix.pdb";

       
        [MenuItem("Trinity/构建热更新DLL")]
        public static void BuildHotfixDLL()
        {
            File.Copy(Path.Combine(ScriptAssembliesDir, HotfixDll), Path.Combine(CodeDir, "Hotfix.dll.bytes"), true);
            File.Copy(Path.Combine(ScriptAssembliesDir, HotfixPdb), Path.Combine(CodeDir, "Hotfix.pdb.bytes"), true);
            Debug.Log("复制Hotfix.dll, Hotfix.pdb到Asset/Res/HotfixDLL完成");
            AssetDatabase.Refresh();

        }


    }
}

