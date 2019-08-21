using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using ETModel;
using UnityEditor;
using GameFramework;

namespace Trinity.Editor
{
    internal class OpcodeInfo
    {
        public string Name;
        public int Opcode;
    }

    public class Proto2CSEditor : EditorWindow
    {
        private const string protoPath = "Proto/";
        private const string clientMessagePath = "Assets/Scripts/Libraries/ET/Module/Message/";
        private const string hotfixMessagePath = "Assets/Scripts/Hotfix/ETNetwork/Module/Message/";
        private static readonly char[] splitChars = { ' ', '\t' };
        private static readonly List<OpcodeInfo> msgOpcode = new List<OpcodeInfo>();

        [MenuItem("Trinity/ETNetwork/Proto2CS")]
        public static void AllProto2CS()
        {
            string rootDir = Environment.CurrentDirectory;
            string protoDir = Utility.Path.GetCombinePath(rootDir, "Proto/");

            string protoc;
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                protoc = Utility.Path.GetCombinePath(protoDir, "protoc.exe");
            }
            else
            {
                protoc = Utility.Path.GetCombinePath(protoDir, "protoc");
            }

            string outerMessageCodePath = Utility.Path.GetCombinePath(rootDir, "Assets", "Scripts", "Libraries", "ET", "Module", "Message/");

            string hotfixMessageCodePath = Utility.Path.GetCombinePath(rootDir, "Assets", "Scripts", "Hotfix", "ETNetwork", "Module", "Message/");

            string argument1 = $"--csharp_out=\"{outerMessageCodePath}\" --proto_path=\"{protoDir}\" OuterMessage.proto";
            string argument2 = $"--csharp_out=\"{hotfixMessageCodePath}\" --proto_path=\"{protoDir}\" HotfixMessage.proto";

            ProcessHelper.Run(protoc, argument1, waitExit: true);
            ProcessHelper.Run(protoc, argument2, waitExit: true);

            // InnerMessage.proto生成cs代码
            InnerProto2CS.Proto2CS();

            msgOpcode.Clear();
            Proto2CS("ETModel", "OuterMessage.proto", clientMessagePath, "OuterOpcode", 100);

            msgOpcode.Clear();
            Proto2CS("ETHotfix", "HotfixMessage.proto", hotfixMessagePath, "HotfixOpcode", 10000);

            UnityEngine.Debug.Log("proto2cs succeed!");

            AssetDatabase.Refresh();
        }

        public static void CommandRun(string exe, string arguments)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo
                {
                    CreateNoWindow = true,
                    FileName = exe,
                    Arguments = arguments,
                    UseShellExecute = true,
                };
                Process p = Process.Start(info);
                p.WaitForExit();
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }

        public static void Proto2CS(string ns, string protoName, string outputPath, string opcodeClassName, int startOpcode, bool isClient = true)
        {
            msgOpcode.Clear();
            string proto = Path.Combine(protoPath, protoName);

            string s = File.ReadAllText(proto);

            StringBuilder sb = new StringBuilder();
            sb.Append("using ETModel;\n");
            sb.Append($"namespace {ns}\n");
            sb.Append("{\n");

            bool isMsgStart = false;

            foreach (string line in s.Split('\n'))
            {
                string newline = line.Trim();

                if (newline == "")
                {
                    continue;
                }

                if (newline.StartsWith("//"))
                {
                    sb.Append($"{newline}\n");
                }

                if (newline.StartsWith("message"))
                {
                    string parentClass = "";
                    isMsgStart = true;
                    string msgName = newline.Split(splitChars, StringSplitOptions.RemoveEmptyEntries)[1];
                    string[] ss = newline.Split(new[] { "//" }, StringSplitOptions.RemoveEmptyEntries);

                    if (ss.Length == 2)
                    {
                        parentClass = ss[1].Trim();
                    }
                    else
                    {
                        parentClass = "";
                    }

                    msgOpcode.Add(new OpcodeInfo() { Name = msgName, Opcode = ++startOpcode });

                    sb.Append($"\t[Message({opcodeClassName}.{msgName})]\n");
                    sb.Append($"\tpublic partial class {msgName} ");
                    if (parentClass != "")
                    {
                        sb.Append($": {parentClass} ");
                    }

                    sb.Append("{}\n\n");
                }

                if (isMsgStart && newline == "}")
                {
                    isMsgStart = false;
                }
            }

            sb.Append("}\n");

            GenerateOpcode(ns, opcodeClassName, outputPath, sb);
        }

        private static void GenerateOpcode(string ns, string outputFileName, string outputPath, StringBuilder sb)
        {
            sb.AppendLine($"namespace {ns}");
            sb.AppendLine("{");
            sb.AppendLine($"\tpublic static partial class {outputFileName}");
            sb.AppendLine("\t{");
            foreach (OpcodeInfo info in msgOpcode)
            {
                sb.AppendLine($"\t\t public const ushort {info.Name} = {info.Opcode};");
            }

            sb.AppendLine("\t}");
            sb.AppendLine("}");

            string csPath = Path.Combine(outputPath, outputFileName + ".cs");
            File.WriteAllText(csPath, sb.ToString());
        }
    }
}
