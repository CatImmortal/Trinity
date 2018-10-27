using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Trinity
{
    /// <summary>
    /// 引用收集器数据
    /// </summary>
    [Serializable]
    public class ReferenceCollectorData
    {
        public string key;
        public Object gameObject;
    }

    /// <summary>
    /// 引用收集器的数据比较器
    /// </summary>
    public class ReferenceCollectorDataComparer : IComparer<ReferenceCollectorData>
    {
        public int Compare(ReferenceCollectorData x, ReferenceCollectorData y)
        {
            return string.Compare(x.key, y.key, StringComparison.Ordinal);
        }
    }

    /// <summary>
    /// 引用收集器
    /// </summary>
    public class ReferenceCollector : MonoBehaviour, ISerializationCallbackReceiver
    {
        public List<ReferenceCollectorData> datas = new List<ReferenceCollectorData>();

        private Dictionary<string, Object> m_Dict = new Dictionary<string, Object>();

#if UNITY_EDITOR
        public void Add(string key, Object obj)
        {
            //获取到datas
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty datasProperty = serializedObject.FindProperty("datas");

            int i;
            for (i = 0; i < datas.Count; i++)
            {
                if (datas[i].key == key)
                {
                    break;
                }
            }

            if (i != datas.Count)
            {
                //覆盖掉原来的
                SerializedProperty element = datasProperty.GetArrayElementAtIndex(i);
                element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
            }
            else
            {
                //新增一个元素
                datasProperty.InsertArrayElementAtIndex(i);
                SerializedProperty element = datasProperty.GetArrayElementAtIndex(i);
                element.FindPropertyRelative("key").stringValue = key;
                element.FindPropertyRelative("gameObject").objectReferenceValue = obj;
            }

            //应用更改
            EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }

        public void Remove(string key)
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty dataProperty = serializedObject.FindProperty("datas");
            int i;
            for (i = 0; i < datas.Count; i++)
            {
                if (datas[i].key == key)
                {
                    break;
                }
            }
            if (i != datas.Count)
            {
                //删除元素
                dataProperty.DeleteArrayElementAtIndex(i);
            }

            //应用更改
            EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();

        }

        public void Clear()
        {
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty dataProperty = serializedObject.FindProperty("datas");
            dataProperty.ClearArray();

            EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }

        public void Sort()
        {
            SerializedObject serializedObject = new SerializedObject(this);
            datas.Sort(new ReferenceCollectorDataComparer());

            EditorUtility.SetDirty(this);
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }
#endif

        /// <summary>
        /// 获取数据
        /// </summary>
        public T Get<T>(string key) where T : class
        {
            Object go;
            if (m_Dict.TryGetValue(key, out go))
            {
                return go as T;
            }

            return null;
        }

        /// <summary>
        /// 获取Object
        /// </summary>
        public Object GetObject(string key)
        {
            Object go;
            if (m_Dict.TryGetValue(key, out go))
            {
                return go;
            }

            return null;
        }

        public Dictionary<string,Object> GetAll()
        {
            return m_Dict;
        }

        public void OnBeforeSerialize()
        {
           
        }

        public void OnAfterDeserialize()
        {
            m_Dict.Clear();
            foreach (ReferenceCollectorData data in datas)
            {
                if (!m_Dict.ContainsKey(data.key))
                {
                    //将数据放入字典里
                    m_Dict.Add(data.key, data.gameObject);
                }
            }
        }
    }
}

