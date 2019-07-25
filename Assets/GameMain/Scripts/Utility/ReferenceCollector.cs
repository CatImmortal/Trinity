using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;
namespace Trinity
{
    /// <summary>
    /// 引用收集器
    /// </summary>
    public class ReferenceCollector : MonoBehaviour
    {


#if UNITY_EDITOR
        [Serializable]
        public class ObjData
        {
            public ObjData()
            {
            }

            public ObjData(string name, Object obj)
            {
                Name = name;
                Obj = obj;
            }

            public string Name;
            public Object Obj;
        }

        public List<ObjData> ObjDatas = new List<ObjData>();
#endif

        [SerializeField]
        private List<Object> m_Objs = new List<Object>();


        public T GetObj<T>(int index) where T : class
        {
            if (index >= m_Objs.Count)
            {
                Debug.LogError("索引无效");
                return null;
            }

            T obj = m_Objs[index] as T;

            if (obj == null)
            {
                Debug.LogError("类型无效");
                return null;
            }

            return obj;
        }


    }
}

