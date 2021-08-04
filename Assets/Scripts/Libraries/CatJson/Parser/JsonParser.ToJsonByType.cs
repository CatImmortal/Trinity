using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace CatJson
{
    public static partial class JsonParser
    {

        /// <summary>
        /// 将指定类型的对象转换为Json文本
        /// </summary>
        public static string ToJson<T>(T obj, bool reflection = true) where T : new()
        {
            return ToJson(obj, typeof(T),reflection);
        }

        /// <summary>
        /// 将指定类型的对象转换为Json文本
        /// </summary>
        public static string ToJson(object obj, Type type, bool reflection = true)
        {
            if (obj is IJsonParserCallbackReceiver receiver)
            {
                //触发转换开始回调
                receiver.OnToJsonStart();
            }

            if (reflection)
            {
                //反射转换
                AppendJsonObject(obj, type, 0);
            }
            else if (GenJsonCodes.ToJsonCodeFuncDict.TryGetValue(type, out Action<object,int> action))
            {
                //使用预生成代码转换
                action(obj, 0);
            }


            if (Util.CachedSB.Length == 0)
            {
                throw new Exception($"没有为{type}类型预生成的转换代码");
            }

            string json = Util.CachedSB.ToString();
            Util.CachedSB.Clear();
            return json;
        }

        /// <summary>
        /// 追加Json数据类对象文本
        /// </summary>
        private static void AppendJsonObject(object obj, Type type, int depth)
        {
            if (!propertyInfoDict.ContainsKey(type) && !fieldInfoDict.ContainsKey(type))
            {
                //初始化反射信息
                AddToReflectionMap(type);
            }

            Util.AppendLine("{");

            bool flag = false;

            propertyInfoDict.TryGetValue(type, out Dictionary<RangeString, PropertyInfo> piDict);
            if (piDict != null)
            {
                foreach (KeyValuePair<RangeString, PropertyInfo> item in piDict)
                {
                    object value = item.Value.GetValue(obj);
                    Type piType = item.Value.PropertyType;
                    string piName = item.Value.Name;

                    if (Util.IsDefaultValue(piType,value))
                    {
                        //默认值跳过序列化
                        continue;
                    }

                    flag = true;

                   

                    AppendJsonKeyValue(piType, piName, value, depth + 1);

                    
                }
            }

            fieldInfoDict.TryGetValue(type, out Dictionary<RangeString, FieldInfo> fiDict);
            if (fiDict != null)
            {
                foreach (KeyValuePair<RangeString, FieldInfo> item in fiDict)
                {
                    object value = item.Value.GetValue(obj);
                    Type fiType = item.Value.FieldType;
                    string fiName = item.Value.Name;

                    if (Util.IsDefaultValue(fiType, value))
                    {
                        //默认值跳过序列化
                        continue;
                    }
                    flag = true;


                    AppendJsonKeyValue(fiType, fiName, value, depth + 1);
                }

            }

            if (flag == true)
            {
                //删掉最后的 , 字符
                Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
            }



            Util.Append("}", depth);

        }

        /// <summary>
        /// 追加单个Json 键值对文本
        /// </summary>
        private static void AppendJsonKeyValue(Type valueType, string name, object value, int depth)
        {
            AppendJsonKey(name, depth);
            AppendJsonValue(valueType, value, depth);
            Util.AppendLine(",");
        }

        /// <summary>
        /// 追加json key文本
        /// </summary>
        public static void AppendJsonKey(string key, int depth)
        {
            Util.Append("\"", depth);
            Util.Append(key);
            Util.Append("\"");
            Util.Append(":");
        }

        /// <summary>
        /// 追加Json 值文本
        /// </summary>
        private static void AppendJsonValue(Type valueType, object value, int depth)
        {
            if (extensionToJsonFuncDict.TryGetValue(valueType, out Action<object> action))
            {
                //自定义转换Json文本方法
                action(value);
                return;
            }

            //根据属性值的不同类型进行序列化
            if (Util.IsNumber(valueType))
            {
                //数字
                Util.Append(value.ToString());
                return;
            }

            if (valueType == typeof(string) || valueType == typeof(char))
            {
                //字符串
                Util.Append("\"");
                Util.Append(value.ToString());
                Util.Append("\"");
                return;
            }

            if (valueType == typeof(bool))
            {
                //bool值
                bool b = (bool)value;
                if (b == true)
                {
                    Util.Append("true");
                }
                else
                {
                    Util.Append("false");
                }
                return;
            }

            if (valueType.IsEnum)
            {
                //枚举
                int enumInt = (int)value;
                Util.Append(enumInt.ToString());
                return;
            }

            if (Util.IsArrayOrList(valueType))
            {
                //数组或List
                AppendJsonArray(valueType,value,depth);
                return;
            }


            if (Util.IsDictionary(valueType))
            {
                //字典
                AppendJsonDict(value, depth);
                return;
            }

            //自定义类对象
            AppendJsonObject(value, valueType, depth);
        }

        /// <summary>
        /// 追加json数组文本
        /// </summary>
        private static void AppendJsonArray(Type valueType, object value, int depth)
        {
            Util.AppendLine("[");

            bool flag = false;

            if (valueType.IsArray)
            {
                Array array = (Array)value;
                for (int i = 0; i < array.Length; i++)
                {
                    flag = true;
                    object element = array.GetValue(i);
                    Util.AppendTab(depth + 1);
                    if (element == null)
                    {
                        Util.Append("null");
                    }
                    else
                    {
                        AppendJsonValue(element.GetType(), element, depth + 1);
                    }
                   

                    Util.AppendLine(",");
                }
            }
            else
            {
                IList list = (IList)value;
                for (int i = 0; i < list.Count; i++)
                {
                    flag = true;
                    object element = list[i];
                    Util.AppendTab(depth + 1);
                    if (element == null)
                    {
                        Util.Append("null");
                    }
                    else
                    {
                        AppendJsonValue(element.GetType(), element, depth + 1);
                    }
                    Util.AppendLine(",");
                }
            }

            if (flag)
            {
                //删掉最后的 , 字符
                Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);

            }
            Util.Append("]", depth);
        }

        /// <summary>
        /// 追加json字典文本
        /// </summary>
        private static void AppendJsonDict(object value, int depth)
        {
            //字典
            IDictionary dict = (IDictionary)value;
            IDictionaryEnumerator enumerator = dict.GetEnumerator();

            Util.AppendLine("{");

            while (enumerator.MoveNext())
            {
                if (enumerator.Value == null)
                {
                    AppendJsonKey(enumerator.Key.ToString(), depth + 1);
                    Util.Append("null");
                    Util.AppendLine(",");
                }
                else
                {
                    AppendJsonKeyValue(enumerator.Value.GetType(), enumerator.Key.ToString(), enumerator.Value, depth + 1);
                }  
                
                
            }

            if (dict.Count > 0)
            {
                //删掉最后的 , 字符
                Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
            }


            Util.Append("}", depth);
        }

        //------------------------为内置基础类型提供append方法，以供生成的代码调用------------------------------------
        public static void AppendJsonValue(bool b, int depth = 0)
        {
            if (b == true)
            {
                Util.Append("true", depth);
            }
            else
            {
                Util.Append("false", depth);
            }

        }

        public static void AppendJsonValue(string s, int depth = 0)
        {
            Util.Append("\"",depth);
            Util.Append(s);
            Util.Append("\"");

        }

        public static void AppendJsonValue(char c, int depth = 0)
        {
            Util.Append("\"", depth);
            Util.Append(c.ToString());
            Util.Append("\"");

        }

        public static void AppendJsonValue(byte b,int depth = 0)
        {
            Util.Append(b.ToString(), depth);
        }

        public static void AppendJsonValue(int i, int depth = 0)
        {
            Util.Append(i.ToString(),depth);
        }

        public static void AppendJsonValue(long l, int depth = 0)
        {
            Util.Append(l.ToString(), depth);

        }

        public static void AppendJsonValue(float f, int depth = 0)
        {
            Util.Append(f.ToString(), depth);
        }

        public static void AppendJsonValue(double d, int depth = 0)
        {
            Util.Append(d.ToString(), depth);
        }

       
    }

}
