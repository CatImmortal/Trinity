using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
//Object并非C#基础中的Object，而是 UnityEngine.Object
using Object = UnityEngine.Object;

namespace Trinity.Editor
{
    //自定义ReferenceCollector类在界面中的显示与功能
    [CustomEditor(typeof(ReferenceCollector))]
    //没有该属性的编辑器在选中多个物体时会提示“Multi-object editing not supported”
    [CanEditMultipleObjects]
    public class ReferenceCollectorEditor : UnityEditor.Editor
    {
        //输入在textfield中的字符串
        private string searchKey
        {
            get
            {
                return _searchKey;
            }
            set
            {
                if (_searchKey != value)
                {
                    _searchKey = value;
                    heroPrefab = referenceCollector.Get<Object>(searchKey);
                }
            }
        }

        private ReferenceCollector referenceCollector;

        private Object heroPrefab;

        private string _searchKey = "";

        /// <summary>
        /// 命名前缀与类型的映射
        /// </summary>
        private List<KeyValuePair<string, string>> prefixesMap = new List<KeyValuePair<string, string>>
        {
            new KeyValuePair<string, string>("Trans","Transform"),
            new KeyValuePair<string, string>("Rect","RectTransform"),
            new KeyValuePair<string, string>("Group","CanvasGroup"),
            new KeyValuePair<string, string>("VGroup","VerticalLayoutGroup"),

            new KeyValuePair<string, string>("Btn","Button"),
            new KeyValuePair<string, string>("Img","Image"),
            new KeyValuePair<string, string>("RImg","RawImage"),
            new KeyValuePair<string, string>("Txt","Text"),
            new KeyValuePair<string, string>("Input","InputField"),

        };




        private void DelNullReference()
        {
            var dataProperty = serializedObject.FindProperty("datas");
            for (int i = dataProperty.arraySize - 1; i >= 0; i--)
            {
                var gameObjectProperty = dataProperty.GetArrayElementAtIndex(i).FindPropertyRelative("obj");
                if (gameObjectProperty.objectReferenceValue == null)
                {
                    dataProperty.DeleteArrayElementAtIndex(i);
                }
            }
        }

        private void OnEnable()
        {
            //将被选中的gameobject所挂载的ReferenceCollector赋值给编辑器类中的ReferenceCollector，方便操作
            referenceCollector = (ReferenceCollector)target;
        }

        public override void OnInspectorGUI()
        {
            //使ReferenceCollector支持撤销操作，还有Redo，不过没有在这里使用
            Undo.RecordObject(referenceCollector, "Changed Settings");
            var datasProperty = serializedObject.FindProperty("datas");
            //开始水平布局，如果是比较新版本学习U3D的，可能不知道这东西，这个是老GUI系统的知识，除了用在编辑器里，还可以用在生成的游戏中
            GUILayout.BeginHorizontal();
            //下面几个if都是点击按钮就会返回true调用里面的东西
            if (GUILayout.Button("添加引用"))
            {
                //添加新的元素，具体的函数注释
                // Guid.NewGuid().GetHashCode().ToString() 就是新建后默认的key
                AddReference(datasProperty, Guid.NewGuid().GetHashCode().ToString(), null);
            }
            if (GUILayout.Button("全部删除"))
            {
                datasProperty.ClearArray();
            }
            if (GUILayout.Button("删除空引用"))
            {
                DelNullReference();
            }
            if (GUILayout.Button("排序"))
            {
                referenceCollector.Sort();
            }
            if (GUILayout.Button("自动绑定组件"))
            {
                AutoBindReference();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            //可以在编辑器中对searchKey进行赋值，只要输入对应的Key值，就可以点后面的删除按钮删除相对应的元素
            searchKey = EditorGUILayout.TextField(searchKey);
            //添加的可以用于选中Object的框，这里的object也是(UnityEngine.Object
            //第三个参数为是否只能引用scene中的Object
            EditorGUILayout.ObjectField(heroPrefab, typeof(Object), false);
            if (GUILayout.Button("删除"))
            {
                referenceCollector.Remove(searchKey);
                heroPrefab = null;
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();

            var delList = new List<int>();
            SerializedProperty property;
            //遍历ReferenceCollector中data list的所有元素，显示在编辑器中
            for (int i = datasProperty.arraySize - 1; i >= 0; i--)
            {
                GUILayout.BeginHorizontal();
                //这里的知识点在ReferenceCollector中有说
                property = datasProperty.GetArrayElementAtIndex(i).FindPropertyRelative("key");
                property.stringValue = EditorGUILayout.TextField(property.stringValue, GUILayout.Width(150));
                property = datasProperty.GetArrayElementAtIndex(i).FindPropertyRelative("obj");
                property.objectReferenceValue = EditorGUILayout.ObjectField(property.objectReferenceValue, typeof(Object), true);
                if (GUILayout.Button("X"))
                {
                    //将元素添加进删除list
                    delList.Add(i);
                }
                GUILayout.EndHorizontal();
            }
            var eventType = Event.current.type;
            //在Inspector 窗口上创建区域，向区域拖拽资源对象，获取到拖拽到区域的对象
            if (eventType == EventType.DragUpdated || eventType == EventType.DragPerform)
            {
                // Show a copy icon on the drag
                DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                if (eventType == EventType.DragPerform)
                {
                    DragAndDrop.AcceptDrag();
                    foreach (var o in DragAndDrop.objectReferences)
                    {
                        AddReference(datasProperty, o.name, o);
                    }
                }

                Event.current.Use();
            }

            //遍历删除list，将其删除掉
            foreach (var i in delList)
            {
                datasProperty.DeleteArrayElementAtIndex(i);
            }
            serializedObject.ApplyModifiedProperties();
            serializedObject.UpdateIfRequiredOrScript();
        }

        //添加元素，具体知识点在ReferenceCollector中说了
        private void AddReference(SerializedProperty dataProperty, string key, Object obj)
        {
            int index = dataProperty.arraySize;
            dataProperty.InsertArrayElementAtIndex(index);
            var element = dataProperty.GetArrayElementAtIndex(index);
            element.FindPropertyRelative("key").stringValue = key;
            element.FindPropertyRelative("obj").objectReferenceValue = obj;
        }

        private void AutoBindReference()
        {
            SerializedProperty datasProperty = serializedObject.FindProperty("datas");
            datasProperty.ClearArray();

            Transform[] childs = referenceCollector.gameObject.GetComponentsInChildren<Transform>();

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
                        AddReference(datasProperty, child.name, child.GetComponent(prefix.Value));
                        break;
                    }
                }
            }
        }
    }
}

