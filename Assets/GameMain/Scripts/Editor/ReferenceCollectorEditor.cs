using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Object = UnityEngine.Object;
using System;

namespace Trinity.Editor
{
    [CustomEditor(typeof(ReferenceCollector))]
    [CanEditMultipleObjects]
    public class ReferenceCollectorEditor : UnityEditor.Editor
    {
        private ReferenceCollector m_ReferenceCollector;

        /// <summary>
        /// 面板上的预制体
        /// </summary>
        private Object m_Prefab;

        
        private string m_searchKey = "";
        private string m_SearchKey
        {
            get
            {
                return m_searchKey;
            }

            set
            {
                if (m_searchKey != value)
                {
                    m_searchKey = value;
                    m_Prefab = m_ReferenceCollector.Get<Object>(m_SearchKey);
                }
            }
        }

        private void OnEnable()
        {
            m_ReferenceCollector = (ReferenceCollector)target;
        }

        public override void OnInspectorGUI()
        {
            //快照设置
            Undo.RecordObject(m_ReferenceCollector, "Changed Settings");

            //获取字段
            SerializedProperty datasProperty = serializedObject.FindProperty("datas");

       
            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("添加引用"))
            {
                AddReference(datasProperty, Guid.NewGuid().GetHashCode().ToString(), null);
            }

            if (GUILayout.Button("删除引用"))
            {
                datasProperty.ClearArray();
            }

            if (GUILayout.Button("删除空引用"))
            {
                DelNullReference();
            }

            if (GUILayout.Button("排序"))
            {
                m_ReferenceCollector.Sort();
            }

            EditorGUILayout.EndHorizontal();
   


  
            EditorGUILayout.BeginHorizontal();

            //绘制SearchKey和Object
            m_SearchKey = EditorGUILayout.TextField(m_SearchKey);
            EditorGUILayout.ObjectField(m_Prefab, typeof(Object), false);

            if (GUILayout.Button("删除"))
            {
                m_ReferenceCollector.Remove(m_SearchKey);
                m_Prefab = null;
            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("注意：");
            EditorGUILayout.LabelField("如果要使用自动生成代码工具来自动获取RC引用到的Object");
            EditorGUILayout.LabelField("那么请按照类型_名称的方式命名，如Text_HeroLevel");

            List<int> delList = new List<int>();
            for (int i = m_ReferenceCollector.datas.Count - 1; i >= 0 ; i--)
            {
                EditorGUILayout.BeginHorizontal();

                //绘制引用
                m_ReferenceCollector.datas[i].key = EditorGUILayout.TextField(m_ReferenceCollector.datas[i].key,GUILayout.Width(150));
                m_ReferenceCollector.datas[i].gameObject = EditorGUILayout.ObjectField(m_ReferenceCollector.datas[i].gameObject,typeof(Object),true);
                if (GUILayout.Button("X"))
                {
                    //添加到要删除的引用列表中
                    delList.Add(i);
                }

                EditorGUILayout.EndHorizontal();
            }

            EventType eventType = Event.current.type;
            if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (eventType == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (Object o in DragAndDrop.objectReferences)
                    {
                        AddReference(datasProperty, o.name, o);
                    }
                }

                Event.current.Use();
            }

            foreach (int i in delList)
            {
                datasProperty.DeleteArrayElementAtIndex(i);
            }
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }

        /// <summary>
        /// 添加引用
        /// </summary>
        private void AddReference(SerializedProperty datasProperty, string key, Object obj)
        {
            ///获取数组长度
            int index = datasProperty.arraySize;

            //插入元素
            datasProperty.InsertArrayElementAtIndex(index);

            //获取元素
            SerializedProperty element = datasProperty.GetArrayElementAtIndex(index);

            //为元素的字段赋值
            element.FindPropertyRelative("key").stringValue = key;
            element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
        }

        /// <summary>
        /// 删除空引用
        /// </summary>
        private void DelNullReference()
        {
            SerializedProperty datasProperty = serializedObject.FindProperty("datas");

            //倒序遍历删除元素
            for (int i = datasProperty.arraySize - 1; i >= 0 ; i--)
            {
                SerializedProperty gameObjectProperty = datasProperty.GetArrayElementAtIndex(i).FindPropertyRelative("gameObject");
                if (gameObjectProperty.objectReferenceValue == null)
                {
                    datasProperty.DeleteArrayElementAtIndex(i);
                }
            }
        }
    }
}

