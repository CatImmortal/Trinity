using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using BindData = Trinity.ComponentAutoBindTool.BindData;

namespace Trinity.Editor
{
    /// <summary>
    /// 实体与界面代码生成器
    /// </summary>
    public class EntityAndUIFormCodeGenerator : EditorWindow
    {
        private enum GenCodeType
        {
            Entity,
            UIForm
        }

        [SerializeField]
        private List<GameObject> m_GameObjects = new List<GameObject>();

        private SerializedObject m_SerializedObject;
        private SerializedProperty m_SerializedProperty;

        private GenCodeType m_GenCodeType;

        /// <summary>
        /// 是否为热更新层代码
        /// </summary>
        private bool m_IsHotfix;

        /// <summary>
        /// 是否生成主体逻辑代码
        /// </summary>
        private bool m_IsGenMainLogicCode = true;

        /// <summary>
        /// 是否生成自动绑定组件代码
        /// </summary>
        private bool m_IsGenAutoBindCode = true;

        /// <summary>
        /// 是否生成实体数据代码
        /// </summary>
        private bool m_IsGenEntityDataCode = true;

        /// <summary>
        /// 是否生成显示实体代码
        /// </summary>
        private bool m_IsGenShowEntityCode = true;

        //各种类型的代码生成后的路径
        private const string EntityCodePath = "Assets/Scripts/GameMain/Entity";
        private const string HotfixEntityCodePath = "Assets/Scripts/Hotfix/Entity";

        private const string UIFormCodePath = "Assets/Scripts/GameMain/UI";
        private const string HotfixUIFormCodePath = "Assets/Scripts/Hotfix/UI";

        [MenuItem("Trinity/代码生成器/实体与界面代码生成器")]
        public static void OpenCodeGeneratorWindow()
        {
            EntityAndUIFormCodeGenerator window = GetWindow<EntityAndUIFormCodeGenerator>(true, "实体与界面代码生成器");
            window.minSize = new Vector2(300f, 300f);
        }

        private void OnEnable()
        {
            m_SerializedObject = new SerializedObject(this);
            m_SerializedProperty = m_SerializedObject.FindProperty("m_GameObjects");
        }

        private void OnGUI()
        {
            //绘制GameObject列表
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_SerializedProperty, true);
            if (EditorGUI.EndChangeCheck())
            {
                m_SerializedObject.ApplyModifiedProperties();
            }

            //绘制自动生成代码类型的弹窗
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("自动生成的代码类型：", GUILayout.Width(140f));
            m_GenCodeType = (GenCodeType)EditorGUILayout.EnumPopup(m_GenCodeType, GUILayout.Width(100f));
            EditorGUILayout.EndHorizontal();

            //绘制代码生成路径文本
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("自动生成的代码路径：", GUILayout.Width(140f));
            switch (m_GenCodeType)
            {
                case GenCodeType.Entity:
                    EditorGUILayout.LabelField(m_IsHotfix ? HotfixEntityCodePath : EntityCodePath);
                    break;
                case GenCodeType.UIForm:
                    EditorGUILayout.LabelField(m_IsHotfix ? HotfixUIFormCodePath : UIFormCodePath);
                    break;
            }
            EditorGUILayout.EndHorizontal();

            //绘制各个选项
            m_IsHotfix = GUILayout.Toggle(m_IsHotfix, "热更新层代码");
            m_IsGenMainLogicCode = GUILayout.Toggle(m_IsGenMainLogicCode, "生成主体逻辑代码");


            EditorGUILayout.BeginHorizontal();
            m_IsGenAutoBindCode = GUILayout.Toggle(m_IsGenAutoBindCode, "生成自动绑定组件代码", GUILayout.Width(150f));

            EditorGUILayout.EndHorizontal();

            if (m_GenCodeType == GenCodeType.Entity)
            {
                m_IsGenEntityDataCode = GUILayout.Toggle(m_IsGenEntityDataCode, "生成实体数据代码");
                m_IsGenShowEntityCode = GUILayout.Toggle(m_IsGenShowEntityCode, "生成快捷显示实体代码");
            }

            //绘制生成代码的按钮
            if (GUILayout.Button("生成代码", GUILayout.Width(100f)))
            {
                if (m_GameObjects.Count == 0)
                {
                    EditorUtility.DisplayDialog("警告", "请选择实体或界面的游戏物体", "OK");
                    return;
                }

                if (m_GenCodeType == GenCodeType.Entity)
                {
                    GenEntityCode();
                }
                else
                {
                    GenUIFormCode();
                }

                AssetDatabase.Refresh();
                EditorUtility.DisplayDialog("提示", "代码生成完毕", "OK");

            }
        }

        private void GenEntityCode()
        {
            //根据是否为热更新实体来决定一些参数
            string codepath = m_IsHotfix ? HotfixEntityCodePath : EntityCodePath;
            string nameSpace = m_IsHotfix ? "Trinity.Hotfix" : "Trinity";
            string logicBaseClass = m_IsHotfix ? "HotfixEntityLogic" : "EntityLogic";

            foreach (GameObject go in m_GameObjects)
            {
                if (m_IsGenMainLogicCode)
                {
                    GenEntityMainLogicCode(codepath, go, nameSpace, logicBaseClass);
                }

                if (m_IsGenEntityDataCode)
                {
                    GenEntityDataCode(codepath, go, nameSpace);
                }

                if (m_IsGenAutoBindCode)
                {
                    GenAutoBindCode(codepath, go, nameSpace, "Logic");
                }

                if (m_IsGenShowEntityCode)
                {
                    GenShowEntityCode(codepath, go, nameSpace);
                }
            }


        }

        private void GenUIFormCode()
        {
            //根据是否为热更新界面来决定一些参数
            string codepath = m_IsHotfix ? HotfixUIFormCodePath : UIFormCodePath;
            string nameSpace = m_IsHotfix ? "Trinity.Hotfix" : "Trinity";
            string logicBaseClass = m_IsHotfix ? "HotfixUGuiForm" : "UGuiForm";


            foreach (GameObject go in m_GameObjects)
            {
                if (m_IsGenMainLogicCode)
                {
                    GenUIFormMainLogicCode(codepath, go, nameSpace, logicBaseClass);
                }

                if (m_IsGenAutoBindCode)
                {
                    GenAutoBindCode(codepath, go, nameSpace);
                }
            }


        }

        private void GenEntityMainLogicCode(string codePath, GameObject go, string nameSpace, string logicBaseClass)
        {
            string initParam = string.Empty;
            string baseInitParam = string.Empty;
            string accessModifier = "protected";
            string entityDataName = go.name + "Data";

            if (m_IsHotfix)
            {
                initParam = "Trinity.HotfixEntity entityLogic, ";
                baseInitParam = "entityLogic, ";
                accessModifier = "public";
            }

            if (!Directory.Exists($"{codePath}/EntityLogic/"))
            {
                Directory.CreateDirectory($"{codePath}/EntityLogic/");
            }

            using (StreamWriter sw = new StreamWriter($"{codePath}/EntityLogic/{go.name}.cs"))
            {
                sw.WriteLine("using System.Collections;");
                sw.WriteLine("using System.Collections.Generic;");
                sw.WriteLine("using UnityEngine;");
                if (!m_IsHotfix)
                {
                    sw.WriteLine("using GameFramework;");
                }
                sw.WriteLine("");

                sw.WriteLine("//自动生成于：" + DateTime.Now);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                sw.WriteLine("");

                //类名
                sw.WriteLine($"\tpublic partial class {go.name}Logic : {logicBaseClass}");
                sw.WriteLine("\t{");
                sw.WriteLine("");

                //定义实体数据字段
                sw.WriteLine($"\t\tprivate {entityDataName} m_{entityDataName};");
                sw.WriteLine("");

                //OnInit方法 获取对象引用
                sw.WriteLine($"\t\t{accessModifier} override void OnInit({initParam}object userdata)");
                sw.WriteLine("\t\t{");
                sw.WriteLine($"\t\t\tbase.OnInit({baseInitParam}userdata);");
                sw.WriteLine("");
                sw.WriteLine($"\t\t\tGetObjects();");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //OnShow方法 获取实体数据
                sw.WriteLine($"\t\t{accessModifier} override void OnShow(object userData)");
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\tbase.OnShow(userData);");
                sw.WriteLine("");
                sw.WriteLine($"\t\t\tm_{entityDataName} = ({entityDataName})userData;");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //OnHide方法 归还实体数据引用
                sw.WriteLine($"\t\t{ accessModifier} override void OnHide(object userData)");
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\tbase.OnHide(userData);");
                sw.WriteLine("");
                sw.WriteLine($"\t\t\tReferencePool.Release(m_{entityDataName});");
                sw.WriteLine("\t\t}");
                sw.WriteLine("\t}");
                sw.WriteLine("}");
            }
        }

        private void GenEntityDataCode(string codePath, GameObject go, string nameSpace)
        {
            string dataBaseClass = "EntityData";
            if (m_IsHotfix)
            {
                dataBaseClass = "HotfixEntityData";
            }
            string entityDataName = go.name + "Data";

            if (!Directory.Exists($"{codePath}/EntityData/"))
            {
                Directory.CreateDirectory($"{codePath}/EntityData/");
            }

            using (StreamWriter sw = new StreamWriter($"{codePath}/EntityData/{ entityDataName}.cs"))
            {
                sw.WriteLine("using UnityEngine;");
                sw.WriteLine("");

                sw.WriteLine("//自动生成于：" + DateTime.Now);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                sw.WriteLine("");

                //类名
                sw.WriteLine($"\tpublic class {entityDataName} : { dataBaseClass}");
                sw.WriteLine("\t{");
                sw.WriteLine("");

                //构造方法
                sw.WriteLine($"\t\tpublic {entityDataName}()");
                sw.WriteLine("\t\t{");

                sw.WriteLine("\t\t}");
                sw.WriteLine("");


                //Fill方法
                sw.WriteLine($"\t\tpublic {entityDataName} Fill(int typeId)");
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\tFill(GameEntry.Entity.GenerateSerialId(),typeId);");
                sw.WriteLine("\t\t\treturn this;");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");

                //Clear方法
                sw.WriteLine("\t\tpublic override void Clear()");
                sw.WriteLine("\t\t{");
                sw.WriteLine("\t\t\tbase.Clear();");
                sw.WriteLine("\t\t}");
                sw.WriteLine("");
                sw.WriteLine("\t}");
                sw.WriteLine("}");
            }
        }

        private void GenShowEntityCode(string codePath, GameObject go, string nameSpace)
        {
            string entityDataName = go.name + "Data";

            if (!Directory.Exists($"{codePath}/ShowEntityExtension/"))
            {
                Directory.CreateDirectory($"{codePath}/ShowEntityExtension/");
            }

            using (StreamWriter sw = new StreamWriter($"{codePath}/ShowEntityExtension/ShowEntityExtension.Show{go.name}.cs"))
            {

                sw.WriteLine("using System.Threading.Tasks;");
                sw.WriteLine("using UnityGameFramework.Runtime;");
                sw.WriteLine("");

                sw.WriteLine("//自动生成于：" + DateTime.Now);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");


                //类名
                sw.WriteLine("\tpublic static partial class ShowEntityExtension");
                sw.WriteLine("\t{");

                //显示实体的方法
                sw.WriteLine($"\t\tpublic static void Show{go.name}(this EntityComponent entityComponent,{entityDataName} data)");
                sw.WriteLine("\t\t{");

                if (m_IsHotfix)
                {
                    sw.WriteLine("\t\t\tTrinity.HotfixEntityData tData = GameFramework.ReferencePool.Acquire<Trinity.HotfixEntityData>();");
                    sw.WriteLine($"\t\t\ttData.Fill(data.Id,data.TypeId,\"{go.name}Logic\",data);");
                    sw.WriteLine("\t\t\ttData.Position = data.Position;");
                    sw.WriteLine("\t\t\ttData.Rotation = data.Rotation;");
                    sw.WriteLine("");

                    sw.WriteLine("\t\t\tentityComponent.ShowHotfixEntity(0, tData);");
                }
                else
                {
                    sw.WriteLine($"\t\t\tentityComponent.ShowEntity(typeof({go.name}Logic), 0, data);");
                }


                sw.WriteLine("\t\t}");

                sw.WriteLine("");

                //显示实体的可等待扩展
                sw.WriteLine($"\t\tpublic static async Task<Entity> AwaitShow{go.name}(this EntityComponent entityComponent,{entityDataName} data)");
                sw.WriteLine("\t\t{");

                if (m_IsHotfix)
                {
                    sw.WriteLine("\t\t\tTrinity.HotfixEntityData tData = GameFramework.ReferencePool.Acquire<Trinity.HotfixEntityData>();");
                    sw.WriteLine($"\t\t\ttData.Fill(data.Id,data.TypeId,\"{go.name}Logic\",data);");
                    sw.WriteLine("\t\t\ttData.Position = data.Position;");
                    sw.WriteLine("\t\t\ttData.Rotation = data.Rotation;");
                    sw.WriteLine("");

                    sw.WriteLine("\t\t\tEntity entity = await entityComponent.AwaitShowHotfixEntity(0, tData);");
                }
                else
                {
                    sw.WriteLine($"\t\t\tEntity entity = await entityComponent.AwaitShowEntity(typeof({go.name}Logic), 0, data);");
                }


                sw.WriteLine("\t\t\treturn entity;");

                sw.WriteLine("\t\t}");

                sw.WriteLine("");

                sw.WriteLine("\t}");

                sw.WriteLine("}");
            }


        }

        private void GenUIFormMainLogicCode(string codePath, GameObject go, string nameSpace, string logicBaseClass)
        {
            string initParam = string.Empty;
            string baseInitParam = string.Empty;
            string accessModifier = "protected";

            if (m_IsHotfix)
            {
                initParam = "Trinity.HotfixUGuiForm uiFormLogic, ";
                baseInitParam = "uiFormLogic, ";
                accessModifier = "public";
            }

            if (!Directory.Exists($"{codePath}/"))
            {
                Directory.CreateDirectory($"{codePath}/");
            }

            using (StreamWriter sw = new StreamWriter($"{codePath}/{go.name}.cs"))
            {
                sw.WriteLine("using System.Collections;");
                sw.WriteLine("using System.Collections.Generic;");
                sw.WriteLine("using UnityEngine;");
                sw.WriteLine("");
                sw.WriteLine("//自动生成于：" + DateTime.Now);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                sw.WriteLine("");

                //类名
                sw.WriteLine($"\tpublic partial class {go.name} : {logicBaseClass}");
                sw.WriteLine("\t{");
                sw.WriteLine("");

                //OnInit
                sw.WriteLine($"\t\t{accessModifier} override void OnInit({initParam}object userdata)");
                sw.WriteLine("\t\t{");
                sw.WriteLine($"\t\t\tbase.OnInit({baseInitParam}userdata);");
                sw.WriteLine("");
                sw.WriteLine($"\t\t\tGetObjects();");
                sw.WriteLine("\t\t}");
                sw.WriteLine("\t}");
                sw.WriteLine("}");
            }
        }

        private void GenAutoBindCode(string codePath, GameObject go, string nameSpace, string nameEx = "")
        {
            ComponentAutoBindTool bindTool = go.GetComponent<ComponentAutoBindTool>();
            if (bindTool == null)
            {
                return;
            }

            if (!Directory.Exists($"{codePath}/BindComponents/"))
            {
                Directory.CreateDirectory($"{codePath}/BindComponents/");
            }

            using (StreamWriter sw = new StreamWriter($"{codePath}/BindComponents/{go.name}{nameEx}.BindComponents.cs"))
            {
                sw.WriteLine("using UnityEngine;");
                if (m_GenCodeType == GenCodeType.UIForm)
                {
                    sw.WriteLine("using UnityEngine.UI;");
                }
                sw.WriteLine("");

                sw.WriteLine("//自动生成于：" + DateTime.Now);

                //命名空间
                sw.WriteLine("namespace " + nameSpace);
                sw.WriteLine("{");
                sw.WriteLine("");

                //类名
                sw.WriteLine($"\tpublic partial class {go.name}{nameEx}");
                sw.WriteLine("\t{");
                sw.WriteLine("");


                foreach (BindData data in bindTool.BindDatas)
                {
                    sw.WriteLine($"\t\tprivate {data.BindCom.GetType().Name} m_{data.Name};");
                }
                sw.WriteLine("");

                sw.WriteLine("\t\tprivate void GetBindComponents(GameObject go)");
                sw.WriteLine("\t\t{");

                //获取绑定的组件
                sw.WriteLine($"\t\t\tComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();;");
                sw.WriteLine("");

                //根据索引获取

                for (int i = 0; i < bindTool.BindDatas.Count; i++)
                {
                    BindData data = bindTool.BindDatas[i];
                    string filedName = $"m_{data.Name}";
                    sw.WriteLine($"\t\t\t{filedName} = autoBindTool.GetBindComponent<{data.BindCom.GetType().Name}>({i});");
                }



                sw.WriteLine("\t\t}");

                sw.WriteLine("");

                sw.WriteLine("\t}");

                sw.WriteLine("}");
            }
        }
    }


}
