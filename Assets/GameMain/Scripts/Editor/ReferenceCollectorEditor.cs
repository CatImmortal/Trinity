using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using Object = UnityEngine.Object;
using ObjData = Trinity.ReferenceCollector.ObjData;

namespace Trinity.Editor
{
    [CustomEditor(typeof(ReferenceCollector))]
    [CanEditMultipleObjects]
    public class ReferenceCollectorEditor : UnityEditor.Editor
    {
        private SerializedProperty m_ObjDatas;
        private SerializedProperty m_Objs;
        private List<ObjData> tempList = new List<ObjData>();
        /// <summary>
        /// 命名前缀与类型的映射
        /// </summary>
        private List<KeyValuePair<string, string>> prefixesMap = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("Trans","Transform"),
            new KeyValuePair<string, string>("OldAnim","Animation"),
            new KeyValuePair<string, string>("NewAnim","Animator"),
            new KeyValuePair<string, string>("Rect","RectTransform"),
            new KeyValuePair<string, string>("Group","CanvasGroup"),
            new KeyValuePair<string, string>("VGroup","VerticalLayoutGroup"),
            new KeyValuePair<string, string>("HGroup","HorizontalLayoutGroup"),
            new KeyValuePair<string, string>("GGroup","GridLayoutGroup"),

            new KeyValuePair<string, string>("Btn","Button"),
            new KeyValuePair<string, string>("Img","Image"),
            new KeyValuePair<string, string>("RImg","RawImage"),
            new KeyValuePair<string, string>("Txt","Text"),
            new KeyValuePair<string, string>("Input","InputField"),

        };

        private void OnEnable()
        {
            m_ObjDatas = serializedObject.FindProperty("ObjDatas");
            m_Objs = serializedObject.FindProperty("m_Objs");
        }


        public override void OnInspectorGUI()
        {
            //绘制功能按钮
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("添加引用"))
            {
                AddReference(Guid.NewGuid().GetHashCode().ToString(), null);
                SyncObjs();
            }

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
                AutoBindReference();
            }

            EditorGUILayout.EndHorizontal();

            DrawKvData();

            DrawDragArea();


            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();

        }

        /// <summary>
        /// 添加引用
        /// </summary>
        private void AddReference(string name, Object obj)
        {
            int index = m_ObjDatas.arraySize;
            m_ObjDatas.InsertArrayElementAtIndex(index);
            SerializedProperty element = m_ObjDatas.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("Name").stringValue = name;
            element.FindPropertyRelative("Obj").objectReferenceValue = obj;

        }

        /// <summary>
        /// 排序
        /// </summary>
        private void Sort()
        {
            ReferenceCollector target = (ReferenceCollector)this.target;

            tempList.Clear();
            foreach (ObjData data in target.ObjDatas)
            {
                tempList.Add(new ObjData(data.Name, data.Obj));
            }
            tempList.Sort((x, y) =>
            {
                return string.Compare(x.Name, y.Name, StringComparison.Ordinal);
            });

            m_ObjDatas.ClearArray();
            foreach (ObjData data in tempList)
            {
                AddReference(data.Name, data.Obj);
            }

            SyncObjs();
        }

        /// <summary>
        /// 全部删除
        /// </summary>
        private void RemoveAll()
        {
            m_ObjDatas.ClearArray();

            SyncObjs();
        }

        /// <summary>
        /// 删除空引用
        /// </summary>
        private void RemoveNull()
        {
            for (int i = m_ObjDatas.arraySize - 1; i >= 0; i--)
            {
                SerializedProperty element = m_ObjDatas.GetArrayElementAtIndex(i).FindPropertyRelative("Obj");
                if (element.objectReferenceValue == null)
                {
                    m_ObjDatas.DeleteArrayElementAtIndex(i);
                }
            }

            SyncObjs();
        }

        /// <summary>
        /// 自动绑定引用
        /// </summary>
        private void AutoBindReference()
        {
            m_ObjDatas.ClearArray();
            Transform[] childs = ((ReferenceCollector)target).gameObject.GetComponentsInChildren<Transform>();

            foreach (Transform child in childs)
            {

                string childPrefix = string.Empty;
                foreach (KeyValuePair<string, string> prefix in prefixesMap)
                {
                    if (child.name.Length <= prefix.Key.Length)
                    {
                        continue;
                    }

                    childPrefix = child.name.Substring(0, prefix.Key.Length);

                    //前缀识别
                    if (childPrefix == prefix.Key)
                    {
                        AddReference(child.name, child.GetComponent(prefix.Value));
                        break;
                    }
                }
            }

            SyncObjs();
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

            for (int i = 0; i < m_ObjDatas.arraySize; i++)
            {

                EditorGUILayout.BeginHorizontal();
                property = m_ObjDatas.GetArrayElementAtIndex(i).FindPropertyRelative("Name");
                property.stringValue = EditorGUILayout.TextField(property.stringValue, GUILayout.Width(150));
                property = m_ObjDatas.GetArrayElementAtIndex(i).FindPropertyRelative("Obj");
                property.objectReferenceValue = EditorGUILayout.ObjectField(property.objectReferenceValue, typeof(Object), true);

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
                m_ObjDatas.DeleteArrayElementAtIndex(needDeleteIndex);
                SyncObjs();
            }

            EditorGUILayout.EndVertical();
        }

        /// <summary>
        /// 绘制资源拖拽区域
        /// </summary>
        private void DrawDragArea()
        {
            //在Inspector 窗口上创建区域，向区域拖拽资源对象，获取到拖拽到区域的对象
            EventType eventType = Event.current.type;
            if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                if (eventType == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (Object item in DragAndDrop.objectReferences)
                    {
                        AddReference(item.name, item);
                    }
                    SyncObjs();
                }

                Event.current.Use();
            }
        }

        /// <summary>
        /// 同步对象数据
        /// </summary>
        private void SyncObjs()
        {
            m_Objs.ClearArray();

            SerializedProperty property;
            for (int i = 0; i < m_ObjDatas.arraySize; i++)
            {
                property = m_ObjDatas.GetArrayElementAtIndex(i).FindPropertyRelative("Obj");
                m_Objs.InsertArrayElementAtIndex(i);
                m_Objs.GetArrayElementAtIndex(i).objectReferenceValue = property.objectReferenceValue;
            }
        }

    }
}

