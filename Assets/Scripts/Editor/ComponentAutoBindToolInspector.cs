using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;
using BindData = Trinity.ComponentAutoBindTool.BindData;

namespace Trinity.Editor
{
    [CustomEditor(typeof(ComponentAutoBindTool))]
    public class ComponentAutoBindToolInspector : UnityEditor.Editor
    {
        private SerializedProperty m_BindDatas;
        private SerializedProperty m_BindComs;
        private List<BindData> m_TempList = new List<BindData>();
        private List<string> m_TempFiledNames = new List<string>();
        private List<string> m_TempComponentTypeNames = new List<string>();


        /// <summary>
        /// 命名前缀与类型的映射
        /// </summary>
        private Dictionary<string, string> m_PrefixesDict = new Dictionary<string, string>()
        {
        {"Trans","Transform" },
        {"OldAnim","Animation"},
        {"NewAnim","Animator"},

        {"Rect","RectTransform"},
        {"Canvas","Canvas"},
        {"Group","CanvasGroup"},
        {"VGroup","VerticalLayoutGroup"},
        {"HGroup","HorizontalLayoutGroup"},
        {"GGroup","GridLayoutGroup"},
        {"TGroup","ToggleGroup"},

        {"Btn","Button"},
        {"Img","Image"},
        {"RImg","RawImage"},
        {"Txt","Text"},
        {"Input","InputField"},
        {"Slider","Slider"},
        {"Mask","Mask"},
        {"Mask2D","RectMask2D"},
        {"Tog","Toggle"},
        {"Sbar","Scrollbar"},
        {"SRect","ScrollRect"},
        {"Drop","Dropdown"},
        };

        private void OnEnable()
        {
            m_BindDatas = serializedObject.FindProperty("BindDatas");
            m_BindComs = serializedObject.FindProperty("m_BindComs");
        }


        public override void OnInspectorGUI()
        {
            //绘制功能按钮
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("排序"))
            {
                Sort();
            }

            if (GUILayout.Button("全部删除"))
            {
                RemoveAll();
            }

            if (GUILayout.Button("删除空引用"))
            {
                RemoveNull();
            }

            if (GUILayout.Button("自动绑定组件"))
            {
                AutoBindComponent();
            }

            EditorGUILayout.EndHorizontal();

            DrawKvData();



            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();

        }


        /// <summary>
        /// 排序
        /// </summary>
        private void Sort()
        {
            ComponentAutoBindTool target = (ComponentAutoBindTool)this.target;

            m_TempList.Clear();
            foreach (BindData data in target.BindDatas)
            {
                m_TempList.Add(new BindData(data.Name, data.BindCom));
            }
            m_TempList.Sort((x, y) =>
            {
                return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
            });

            m_BindDatas.ClearArray();
            foreach (BindData data in m_TempList)
            {
                AddBindData(data.Name, data.BindCom);
            }

            SyncBindComs();
        }

        /// <summary>
        /// 全部删除
        /// </summary>
        private void RemoveAll()
        {
            m_BindDatas.ClearArray();

            SyncBindComs();
        }

        /// <summary>
        /// 删除空引用
        /// </summary>
        private void RemoveNull()
        {
            for (int i = m_BindDatas.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty element = m_BindDatas.GetArrayElementAtIndex(i).FindPropertyRelative("Obj");
                if (element.objectReferenceValue == null)
                {
                    m_BindDatas.DeleteArrayElementAtIndex(i);
                }
            }

            SyncBindComs();
        }

        /// <summary>
        /// 自动绑定组件
        /// </summary>
        private void AutoBindComponent()
        {
            m_BindDatas.ClearArray();
            Transform[] childs = ((ComponentAutoBindTool)target).gameObject.GetComponentsInChildren<Transform>();

            foreach (Transform child in childs)
            {
                m_TempFiledNames.Clear();
                m_TempComponentTypeNames.Clear();

                if (IsValidBind(child, m_TempFiledNames, m_TempComponentTypeNames))
                {
                    for (int i = 0; i < m_TempFiledNames.Count; i++)
                    {
                        Component com = child.GetComponent(m_TempComponentTypeNames[i]);
                        if (com == null)
                        {
                            Debug.LogError($"{child.name}上不存在{m_TempComponentTypeNames[i]}的组件");
                        }
                        else
                        {
                            AddBindData(m_TempFiledNames[i], child.GetComponent(m_TempComponentTypeNames[i]));
                        }

                    }
                }
            }

            SyncBindComs();
        }

        /// <summary>
        /// 是否为有效绑定
        /// </summary>
        private bool IsValidBind(Transform target, List<string> filedNames, List<string> componentTypeNames)
        {
            string[] strArray = target.name.Split('_');

            if (strArray.Length == 1)
            {
                return false;
            }

            string filedName = strArray[strArray.Length - 1];

            for (int i = 0; i < strArray.Length - 1; i++)
            {
                string str = strArray[i];
                string comName;
                if (m_PrefixesDict.TryGetValue(str, out comName))
                {
                    filedNames.Add($"{str}_{filedName}");
                    componentTypeNames.Add(comName);
                }
                else
                {
                    Debug.LogError($"{target.name}的命名中{str}不存在对应的组件类型，绑定失败");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 绘制键值对数据
        /// </summary>
        private void DrawKvData()
        {
            //绘制key value数据

            int needDeleteIndex = -1;

            EditorGUILayout.BeginVertical();
            SerializedProperty property;

            for (int i = 0; i < m_BindDatas.arraySize; i++)
            {

                EditorGUILayout.BeginHorizontal();
                property = m_BindDatas.GetArrayElementAtIndex(i).FindPropertyRelative("Name");
                property.stringValue = EditorGUILayout.TextField(property.stringValue, GUILayout.Width(150));
                property = m_BindDatas.GetArrayElementAtIndex(i).FindPropertyRelative("BindCom");
                property.objectReferenceValue = EditorGUILayout.ObjectField(property.objectReferenceValue, typeof(Component), true);

                if (GUILayout.Button("X"))
                {
                    //将元素下标添加进删除list
                    needDeleteIndex = i;
                }
                EditorGUILayout.EndHorizontal();
            }

            //删除data
            if (needDeleteIndex != -1)
            {
                m_BindDatas.DeleteArrayElementAtIndex(needDeleteIndex);
                SyncBindComs();
            }

            EditorGUILayout.EndVertical();
        }



        /// <summary>
        /// 添加绑定数据
        /// </summary>
        private void AddBindData(string name, Component bindCom)
        {
            int index = m_BindDatas.arraySize;
            m_BindDatas.InsertArrayElementAtIndex(index);
            SerializedProperty element = m_BindDatas.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("Name").stringValue = name;
            element.FindPropertyRelative("BindCom").objectReferenceValue = bindCom;

        }

        /// <summary>
        /// 同步组件数据
        /// </summary>
        private void SyncBindComs()
        {
            m_BindComs.ClearArray();

            SerializedProperty property;
            for (int i = 0; i < m_BindDatas.arraySize; i++)
            {
                property = m_BindDatas.GetArrayElementAtIndex(i).FindPropertyRelative("BindCom");
                m_BindComs.InsertArrayElementAtIndex(i);
                m_BindComs.GetArrayElementAtIndex(i).objectReferenceValue = property.objectReferenceValue;
            }
        }



    }
}

