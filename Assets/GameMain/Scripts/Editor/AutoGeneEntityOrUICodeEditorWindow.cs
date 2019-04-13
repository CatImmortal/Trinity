using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using GameFramework;
using Trinity;
using Object = UnityEngine.Object;

namespace Trinity.Editor
{
    public enum AutoGeneType
    {
        Entity,
        HotfixEntity,
        UIForm,
        HotfixUIForm
    }

    public class AutoGeneEntityOrUICodeEditorWindow : EditorWindow
    {

        [SerializeField]
        private List<GameObject> m_GameObjectList = new List<GameObject>();

        private SerializedObject m_SerializedObject;
        private SerializedProperty m_SerializedProperty;

        private AutoGeneType m_TargetType = AutoGeneType.Entity;

        //各种类型的代码生成后的路径
        private const string m_EntityCodePath = "Assets/GameMain/Scripts/Entity";
        private const string m_HotfixEntityCodePath = "Assets/GameMain/Scripts/Hotfix/Entity";
        private const string m_UIFormCodePath = "Assets/GameMain/Scripts/UI/Customs";
        private const string m_HotfixUIFormCodePath = "Assets/GameMain/Scripts/Hotfix/UI/UIForm";

        private void OnEnable()
        {
            m_GameObjectList.Clear();
            m_SerializedObject = new SerializedObject(this);
            m_SerializedProperty = m_SerializedObject.FindProperty("m_GameObjectList");
        }

        [MenuItem("Trinity/自动生成实体或界面的代码")]
        public static void OpenAutoGeneWindow()
        {
            AutoGeneEntityOrUICodeEditorWindow window = GetWindow<AutoGeneEntityOrUICodeEditorWindow>(true, "自动生成实体或界面的代码");
            window.minSize = new Vector2(460f, 400f);
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
            m_TargetType = (AutoGeneType)EditorGUILayout.EnumPopup(m_TargetType, GUILayout.Width(100f));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("自动生成的代码路径：", GUILayout.Width(140f));
            switch (m_TargetType)
            {
                case AutoGeneType.Entity:
                    EditorGUILayout.LabelField(m_EntityCodePath);
                    break;
                case AutoGeneType.HotfixEntity:
                    EditorGUILayout.LabelField(m_HotfixEntityCodePath);
                    break;
                case AutoGeneType.UIForm:
                    EditorGUILayout.LabelField(m_UIFormCodePath);
                    break;
                case AutoGeneType.HotfixUIForm:
                    EditorGUILayout.LabelField(m_HotfixUIFormCodePath);
                    break;
                default:
                    break;
            }

            EditorGUILayout.EndHorizontal();





            //生成代码
            if (GUILayout.Button("生成模板代码", GUILayout.Width(100f)))
            {
                if (m_GameObjectList.Count == 0)
                {
                    EditorUtility.DisplayDialog("警告", "请选择实体或界面的预制体", "OK");
                    return;
                }

                switch (m_TargetType)
                {
                    case AutoGeneType.Entity:
                        AutoGeneEntityCode(false);
                        break;
                    case AutoGeneType.HotfixEntity:
                        AutoGeneEntityCode(true);
                        break;
                    case AutoGeneType.UIForm:
                        AutoGeneUIFormCode(false);
                        break;
                    case AutoGeneType.HotfixUIForm:
                        AutoGeneUIFormCode(true);
                        break;
                    default:
                        break;
                }

                Debug.Log("代码自动生成完毕");
                AssetDatabase.Refresh();
            }




            EditorGUILayout.LabelField("");


            if (GUILayout.Button("生成简便显示实体方法的代码", GUILayout.Width(200f)))
            {
                if (m_TargetType != AutoGeneType.Entity && m_TargetType != AutoGeneType.HotfixEntity)
                {
                    EditorUtility.DisplayDialog("警告", "请选择生成类型为实体类型", "OK");
                    return;
                }

                if (m_TargetType == AutoGeneType.Entity)
                {
                    AutoGeneShowEntityCode(false);
                }
                else
                {
                    AutoGeneShowEntityCode(true);
                }

                Debug.Log("简便显示实体方法的代码自动生成完毕");
                AssetDatabase.Refresh();
            }

            EditorGUILayout.LabelField("注意：请先为要生成简便显示方法代码的实体生成模板代码");
            EditorGUILayout.LabelField("如果未选择实体预制体");
            EditorGUILayout.LabelField("那么将默认为Assets/GameMain/Entities目录下所有实体生成简便显示方法");


        }

        /// <summary>
        /// 自动生成实体代码
        /// </summary>
        private void AutoGeneEntityCode(bool isHotfix)
        {
            //根据是否为热更新实体来决定一些参数
            string codepath = isHotfix ? m_HotfixEntityCodePath : m_EntityCodePath;
            string nameSpace = isHotfix ? "Trinity.Hotfix" : "Trinity";
            string dataBaseClass = isHotfix ? "HotfixEntityData" : "EntityData";
            string logicBaseClass = isHotfix ? "HotfixEntity" : "Entity";
            string accessModifier = isHotfix ? "public" : "protected";
            string OnInitParams = isHotfix ? "Trinity.HotfixEntity entityLogic, object userData" : "object userData";
            string BaseOnInitParams = isHotfix ? "entityLogic, userData" : "userData";
            string GetComponent = isHotfix ? "EntityLogic.GetComponent" : "GetComponent";

            foreach (GameObject gameObject in m_GameObjectList)
            {
                string entityDataName = gameObject.name + "Data";

                //生成实体数据的代码文件
                using (StreamWriter sw = new StreamWriter(Utility.Text.Format("{0}/EntityData/{1}.cs", codepath, entityDataName)))
                {
                    sw.WriteLine("using UnityEngine;");
                    sw.WriteLine("");

                    sw.WriteLine("//自动生成于：" + DateTime.Now);

                    //命名空间
                    sw.WriteLine("namespace " + nameSpace);
                    sw.WriteLine("{");
                    sw.WriteLine("");

                    //类名
                    sw.WriteLine(Utility.Text.Format("\tpublic class {0} : {1}", entityDataName, dataBaseClass));
                    sw.WriteLine("\t{");
                    sw.WriteLine("");

                    //构造方法
                    sw.WriteLine(Utility.Text.Format("\t\tpublic {0}()", entityDataName));
                    sw.WriteLine("\t\t{");

                    sw.WriteLine("\t\t}");
                    sw.WriteLine("");


                    //Fill方法
                    sw.WriteLine(Utility.Text.Format("\t\tpublic {0} Fill(int typeId)", entityDataName));
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

                //生成实体逻辑的代码文件
                using (StreamWriter sw = new StreamWriter(Utility.Text.Format("{0}/EntityLogic/{1}.cs", codepath, gameObject.name)))
                {
                    sw.WriteLine("using UnityEngine;");
                    if (!isHotfix)
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
                    sw.WriteLine(Utility.Text.Format("\tpublic class {0} : {1}", gameObject.name, logicBaseClass));
                    sw.WriteLine("\t{");
                    sw.WriteLine("");

                    //定义实体数据字段
                    sw.WriteLine(Utility.Text.Format("\t\tprivate {0} m_{1};", entityDataName, entityDataName));

                    //RC上的Object引用获取
                    ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
                    if (rc != null)
                    {
                        Dictionary<string, Object> dict = rc.GetAll();
                        Dictionary<string, string> resultDict = new Dictionary<string, string>();

                        foreach (KeyValuePair<string, Object> obj in dict)
                        {
                            //RC引用的组件命名规则为：类型_名称，如：Text_HeroLevel
                            string[] key = obj.Key.Split('_');

                            if (key.Length > 2)
                            {
                                Debug.Log(obj.Key + "分割线超过1个");
                            }

                            if (key.Length < 2)
                            {
                                Debug.Log("注意: " + obj.Key + " 分割线不足1个");
                            }
                            else
                            {
                                if (resultDict.ContainsKey(key[1]))
                                {
                                    Debug.LogError("发现重名: " + obj.Key);
                                    break;
                                }

                                resultDict.Add(key[1], key[0]);
                            }

                        }

                        //定义RC引用到的Object的字段
                        foreach (KeyValuePair<string, string> result in resultDict)
                        {
                            sw.WriteLine("\t\tprivate {0} m_{1};", result.Value, result.Key);
                        }
                        sw.WriteLine("");
                        sw.WriteLine(Utility.Text.Format("\t\t{0} override void OnInit({1})", accessModifier, OnInitParams));
                        sw.WriteLine("\t\t{");

                        sw.WriteLine(Utility.Text.Format("\t\t\tbase.OnInit({0});", BaseOnInitParams));

                        sw.WriteLine("");

                        //获取RC上的Object
                        sw.WriteLine(Utility.Text.Format("\t\t\tReferenceCollector rc = {0}<ReferenceCollector>();", GetComponent));
                        sw.WriteLine("");
                        foreach (KeyValuePair<string, string> result in resultDict)
                        {
                            string name = Utility.Text.Format("m_{0}", result.Key);
                            string param = Utility.Text.Format("{0}_{1}", result.Value, result.Key);
                            sw.WriteLine(Utility.Text.Format("\t\t\t{0} = rc.Get<{1}>(\"{2}\");", name, result.Value, param));
                        }

                        sw.WriteLine("\t\t}");
                        
                    }
                    sw.WriteLine("");
                    //OnShow方法 获取实体数据
                    sw.WriteLine(Utility.Text.Format("\t\t{0} override void OnShow(object userData)", accessModifier));
                    sw.WriteLine("\t\t{");

                    sw.WriteLine("\t\t\tbase.OnShow(userData);");
                    sw.WriteLine(Utility.Text.Format("\t\t\tm_{0} = ({1})userData;", entityDataName, entityDataName));

                    sw.WriteLine("\t\t}");
                    sw.WriteLine("");

                    //OnHide方法 归还实体数据引用
                    sw.WriteLine(Utility.Text.Format("\t\t{0} override void OnHide(object userData)", accessModifier));
                    sw.WriteLine("\t\t{");

                    sw.WriteLine("\t\t\tbase.OnHide(userData);");
                    sw.WriteLine(Utility.Text.Format("\t\t\tReferencePool.Release(m_{0});", entityDataName));

                    sw.WriteLine("\t\t}");

                    sw.WriteLine("\t}");

                    sw.WriteLine("}");
                }
            }


        }

        /// <summary>
        /// 自动生成界面代码
        /// </summary>
        private void AutoGeneUIFormCode(bool isHotfix)
        {
            //根据是否为热更新界面来决定一些参数
            string codepath = isHotfix ? m_HotfixUIFormCodePath : m_UIFormCodePath;
            string nameSpace = isHotfix ? "Trinity.Hotfix" : "Trinity";
            string logicBaseClass = isHotfix ? "HotfixUGuiForm" : "UGuiForm";
            string accessModifier = isHotfix ? "public" : "protected";
            string OnInitParams = isHotfix ? "Trinity.HotfixUGuiForm uiFormLogic, object userData" : "object userData";
            string BaseOnInitParams = isHotfix ? "uiFormLogic, userData" : "userData";
            string GetComponent = isHotfix ? "UIFormLogic.GetComponent" : "GetComponent";

            foreach (GameObject gameObject in m_GameObjectList)
            {
                using (StreamWriter sw = new StreamWriter(Utility.Text.Format("{0}/{1}.cs", codepath, gameObject.name)))
                {
                    sw.WriteLine("using UnityEngine;");
                    sw.WriteLine("using UnityEngine.UI;");
                    sw.WriteLine("");

                    sw.WriteLine("//自动生成于：" + DateTime.Now);

                    //命名空间
                    sw.WriteLine("namespace " + nameSpace);
                    sw.WriteLine("{");

                    //类名
                    sw.WriteLine(Utility.Text.Format("\tpublic class {0} : {1}", gameObject.name, logicBaseClass));
                    sw.WriteLine("\t{");

                    //Object获取
                    ReferenceCollector rc = gameObject.GetComponent<ReferenceCollector>();
                    if (rc != null)
                    {
                        Dictionary<string, Object> dict = rc.GetAll();
                        Dictionary<string, string> resultDict = new Dictionary<string, string>();

                        foreach (KeyValuePair<string, Object> obj in dict)
                        {
                            //RC引用的组件命名规则为：类型_名称，如：Text_HeroLevel
                            string[] key = obj.Key.Split('_');

                            if (key.Length > 2)
                            {
                                Debug.Log(obj.Key + "分割线超过1个");
                            }

                            if (key.Length < 2)
                            {
                                Debug.Log("注意: " + obj.Key + " 分割线不足1个");
                            }
                            else
                            {
                                if (resultDict.ContainsKey(key[1]))
                                {
                                    Debug.LogError("发现重名: " + obj.Key);
                                    break;
                                }

                                resultDict.Add(key[1], key[0]);
                            }

                        }

                        //定义RC引用到的Object的字段
                        foreach (KeyValuePair<string, string> result in resultDict)
                        {
                            sw.WriteLine("\t\tprivate {0} m_{1};", result.Value, result.Key);
                        }
                        sw.WriteLine("");
                        //OnInit方法
                        sw.WriteLine(Utility.Text.Format("\t\t{0} override void OnInit({1})", accessModifier, OnInitParams));
                        sw.WriteLine("\t\t{");

                        sw.WriteLine(Utility.Text.Format("\t\t\tbase.OnInit({0});", BaseOnInitParams));

                        sw.WriteLine("");

                        //获取RC上的Object
                        sw.WriteLine(Utility.Text.Format("\t\t\tReferenceCollector rc = {0}<ReferenceCollector>();", GetComponent));
                        sw.WriteLine("");
                        foreach (KeyValuePair<string, string> result in resultDict)
                        {
                            string name = Utility.Text.Format("m_{0}", result.Key);
                            string param = Utility.Text.Format("{0}_{1}", result.Value, result.Key);
                            sw.WriteLine(Utility.Text.Format("\t\t\t{0} = rc.Get<{1}>(\"{2}\");", name, result.Value, param));
                        }

                        sw.WriteLine("\t\t}");
                    }


                    sw.WriteLine("\t}");

                    sw.WriteLine("}");
                }
            }
        }

        /// <summary>
        /// 自动生成简便显示实体方法的代码
        /// </summary>
        private void AutoGeneShowEntityCode(bool isHotfix)
        {
            //根据是否为热更新实体来决定一些参数
            string codepath = isHotfix ? m_HotfixEntityCodePath : m_EntityCodePath;
            string nameSpace = isHotfix ? "Trinity.Hotfix" : "Trinity";

            //实体逻辑类名-实体数据类名的字典
            Dictionary<string, string> entityDict = new Dictionary<string, string>();


            if (m_GameObjectList.Count > 0)
            {
                foreach (GameObject gameObject in m_GameObjectList)
                {
                    entityDict.Add(gameObject.name, gameObject.name + "Data");
                }
            }
            else
            {
                //如果没选择实体的预制体，则默认为所有实体生成简便显示方法
                string[] paths = Directory.GetFiles("Assets/GameMain/Entities", "*.prefab", SearchOption.AllDirectories);
                for (int i = 0; i < paths.Length; i++)
                {
                    GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(Utility.Path.GetRegularPath(paths[i]));
                    entityDict.Add(go.name, go.name + "Data");
                }
            }

            if (isHotfix)
            {
                //生成简便显示热更新实体的封装方法
                using (StreamWriter sw = new StreamWriter(Utility.Text.Format("{0}/{1}.cs", codepath, "ShowEntityExtension")))
                {
                    sw.WriteLine("using UnityGameFramework.Runtime;");
                    sw.WriteLine("");

                    sw.WriteLine("//自动生成于：" + DateTime.Now);

                    //命名空间
                    sw.WriteLine("namespace " + nameSpace);
                    sw.WriteLine("{");

                    //类名
                    sw.WriteLine("\tpublic static class ShowEntityExtension");
                    sw.WriteLine("\t{");

                    //显示实体的方法
                    foreach (KeyValuePair<string, string> item in entityDict)
                    {
                        sw.WriteLine(Utility.Text.Format("\t\tpublic static void Show{0}(this EntityComponent entityComponent,{1} data)", item.Key, item.Value));
                        sw.WriteLine("\t\t{");

                        sw.WriteLine("\t\t\tTrinity.HotfixEntityData tData = GameFramework.ReferencePool.Acquire<Trinity.HotfixEntityData>();");
                        sw.WriteLine(Utility.Text.Format("\t\t\ttData.Fill(data.Id,data.TypeId,\"{0}\",data);", item.Key));
                        sw.WriteLine("\t\t\ttData.Position = data.Position;");
                        sw.WriteLine("\t\t\ttData.Rotation = data.Rotation;");
                        sw.WriteLine("");

                        sw.WriteLine(Utility.Text.Format("\t\t\tentityComponent.ShowHotfixEntity(0, tData);"));

                        sw.WriteLine("\t\t}");

                        sw.WriteLine("");
                    }

                    sw.WriteLine("\t}");

                    sw.WriteLine("}");

                }
            }
            else
            {
                //生成简便显示实体的封装方法
                using (StreamWriter sw = new StreamWriter(Utility.Text.Format("{0}/{1}.cs", codepath, "ShowEntityExtension")))
                {
                    sw.WriteLine("using UnityGameFramework.Runtime;");
                    sw.WriteLine("");

                    sw.WriteLine("//自动生成于：" + DateTime.Now);

                    //命名空间
                    sw.WriteLine("namespace " + nameSpace);
                    sw.WriteLine("{");


                    //类名
                    sw.WriteLine("\tpublic static class ShowEntityExtension");
                    sw.WriteLine("\t{");

                    //显示实体的方法
                    foreach (KeyValuePair<string, string> item in entityDict)
                    {
                        sw.WriteLine(Utility.Text.Format("\t\tpublic static void Show{0}(this EntityComponent entityComponent,{1} data)", item.Key, item.Value));
                        sw.WriteLine("\t\t{");

                        sw.WriteLine("\t\t\tentityComponent.ShowEntity(typeof({0}), 0, data);", item.Key);

                        sw.WriteLine("\t\t}");

                        sw.WriteLine("");
                    }

                    sw.WriteLine("\t}");

                    sw.WriteLine("}");
                }
            }

        }
    }
}


