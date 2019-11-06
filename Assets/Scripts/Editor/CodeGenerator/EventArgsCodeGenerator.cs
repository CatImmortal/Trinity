using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

namespace Trinity.Editor
{

    /// <summary>
    /// 事件参数类代码生成器
    /// </summary>
    public class EventArgsCodeGenerator : EditorWindow
    {
        /// <summary>
        /// 事件参数数据
        /// </summary>
        [Serializable]
        private class EventArgsData
        {
            public string Type;
            public string Name;
            public EventArgType TypeEnum;
            public EventArgsData()
            {

            }

            public EventArgsData(string type, string name)
            {
                Type = type;
                Name = name;
            }


        }

        private enum EventArgType
        {
            Object,
            Int,
            Float,
            Bool,
            Char,
            String,

            UnityObject,
            GameObject,
            Transform,
            Vector2,
            Vector3,
            Quaternion,

            Other,
        }

        [MenuItem("Trinity/代码生成器/事件参数类代码生成器")]
        public static void OpenAutoGenWindow()
        {
            EventArgsCodeGenerator window = GetWindow<EventArgsCodeGenerator>(true, "事件参数类代码生成器");
            window.minSize = new Vector2(600f, 600f);
        }

        /// <summary>
        /// 事件参数数据列表
        /// </summary>
        [SerializeField]
        private List<EventArgsData> m_EventArgsDatas = new List<EventArgsData>();

        /// <summary>
        /// 是否是热更新层事件
        /// </summary>
        private bool m_IsHotfixEvent = false;

        /// <summary>
        /// 事件参数类名
        /// </summary>
        private string m_ClassName;

        //事件代码生成后的路径
        private const string EventCodePath = "Assets/Scripts/GameMain/EventArgs";
        private const string HotfixEventCodePath = "Assets/Scripts/Hotfix/EventArgs";
        private void OnEnable()
        {
            m_EventArgsDatas.Clear();
            m_ClassName = "EventArgs";
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("事件参数类名：", GUILayout.Width(140f));
            m_ClassName = EditorGUILayout.TextField(m_ClassName);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("热更新层事件：", GUILayout.Width(140f));
            m_IsHotfixEvent = EditorGUILayout.Toggle(m_IsHotfixEvent);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("自动生成的代码路径：", GUILayout.Width(140f));
            EditorGUILayout.LabelField(m_IsHotfixEvent ? HotfixEventCodePath : EventCodePath);
            EditorGUILayout.EndHorizontal();

            //绘制事件参数相关按钮
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加事件参数", GUILayout.Width(140f)))
            {
                m_EventArgsDatas.Add(new EventArgsData(null, null));
            }
            if (GUILayout.Button("删除所有事件参数", GUILayout.Width(140f)))
            {
                m_EventArgsDatas.Clear();
            }
            if (GUILayout.Button("删除空事件参数", GUILayout.Width(140f)))
            {
                for (int i = m_EventArgsDatas.Count - 1; i >= 0; i--)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    if (string.IsNullOrWhiteSpace(data.Name))
                    {
                        m_EventArgsDatas.RemoveAt(i);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();

            //绘制事件参数数据
            for (int i = 0; i < m_EventArgsDatas.Count; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EventArgsData data = m_EventArgsDatas[i];
                EditorGUILayout.LabelField("参数类型：", GUILayout.Width(70f));
                data.TypeEnum = (EventArgType)EditorGUILayout.EnumPopup(data.TypeEnum, GUILayout.Width(100f));
                switch (data.TypeEnum)
                {
                    case EventArgType.Object:
                    case EventArgType.Int:
                    case EventArgType.Float:
                    case EventArgType.Bool:
                    case EventArgType.Char:
                    case EventArgType.String:
                        data.Type = data.TypeEnum.ToString().ToLower();
                        break;

                    case EventArgType.UnityObject:
                        data.Type = "UnityEngine.Object";
                        break;

                    case EventArgType.Other:
                        data.Type = EditorGUILayout.TextField(data.Type, GUILayout.Width(140f));
                        break;

                    default:
                        data.Type = data.TypeEnum.ToString();
                        break;
                }
                EditorGUILayout.LabelField("参数字段名：", GUILayout.Width(70f));
                data.Name = EditorGUILayout.TextField(data.Name, GUILayout.Width(140f));
                EditorGUILayout.EndHorizontal();
            }

            //生成事件参数类代码
            if (GUILayout.Button("生成事件参数类代码", GUILayout.Width(210f)))
            {
                GenEventCode();
                AssetDatabase.Refresh();
            }
        }

        private void GenEventCode()
        {
            //根据是否为热更新层事件来决定一些参数
            string codePath = m_IsHotfixEvent ? HotfixEventCodePath : EventCodePath;
            string nameSpace = m_IsHotfixEvent ? "Trinity.Hotfix" : "Trinity";
            string baseClass = m_IsHotfixEvent ? "HotfixGameEventArgs" : "GameEventArgs";

            if (!Directory.Exists($"{codePath}/"))
            {
                Directory.CreateDirectory($"{codePath}/");
            }

            using (StreamWriter sw = new StreamWriter($"{codePath}/{m_ClassName}.cs"))
            {
                sw.WriteLine("using UnityEngine;");
                sw.WriteLine("using GameFramework.Event;");
                sw.WriteLine("");

                sw.WriteLine("//自动生成于：" + DateTime.Now);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                sw.WriteLine("");

                //类名
                sw.WriteLine($"\tpublic class {m_ClassName} : {baseClass}");
                sw.WriteLine("\t{");
                sw.WriteLine("");

                //事件编号
                sw.WriteLine($"\t\tpublic static readonly int EventId = typeof({m_ClassName}).GetHashCode();");
                sw.WriteLine("");
                sw.WriteLine("\t\tpublic override int Id");
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\tget");
                sw.WriteLine("\t\t\t{");
                sw.WriteLine("\t\t\t\treturn EventId;");
                sw.WriteLine("\t\t\t}");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //事件参数
                for (int i = 0; i < m_EventArgsDatas.Count; i++)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    data.Name = data.Name[0].ToString().ToUpper() + data.Name.Substring(1);
                    sw.WriteLine($"\t\tpublic {data.Type} {data.Name}");
                    sw.WriteLine("\t\t{");
                    sw.WriteLine("\t\t\tget;");
                    sw.WriteLine("\t\t\tprivate set;");
                    sw.WriteLine("\t\t}");
                    sw.WriteLine("");
                }

                //清空参数数据方法
                sw.WriteLine($"\t\tpublic override void Clear()");
                sw.WriteLine("\t\t{");
                for (int i = 0; i < m_EventArgsDatas.Count; i++)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    sw.WriteLine($"\t\t\t{data.Name} = default({data.Type});");
                }
                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //填充参数数据方法
                sw.Write($"\t\tpublic {m_ClassName} Fill(");
                for (int i = 0; i < m_EventArgsDatas.Count; i++)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    sw.Write($"{data.Type} {data.Name.ToLower()}");
                    if (i != m_EventArgsDatas.Count - 1)
                    {
                        sw.Write(",");
                    }
                }
                sw.WriteLine(")");
                sw.WriteLine("\t\t{");
                for (int i = 0; i < m_EventArgsDatas.Count; i++)
                {
                    EventArgsData data = m_EventArgsDatas[i];
                    sw.WriteLine($"\t\t\t{data.Name} = {data.Name.ToLower()};");
                }
                sw.WriteLine("\t\t\treturn this;");
                sw.WriteLine("\t\t}");



                sw.WriteLine("\t}");
                sw.WriteLine("}");

            }
        }
    }



}

