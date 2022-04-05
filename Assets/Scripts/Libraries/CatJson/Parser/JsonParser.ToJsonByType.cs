using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
#if FUCK_LUA
using ILRuntime.CLR.TypeSystem;
using ILRuntime.Reflection;
using ILRuntime.Runtime.Intepreter;
#endif

namespace CatJson
{
    public static partial class JsonParser
    {

        /// <summary>
        /// 将指定类型的对象转换为Json文本
        /// </summary>
        public static string ToJson<T>(T obj, bool reflection = true)
        {
            string json = ToJson(obj, typeof(T), reflection);
            return json;
        }
        
        /// <summary>
        /// 将指定类型的对象转换为Json文本
        /// </summary>
        public static string ToJson(object obj,Type type, bool reflection = true)
        {
            if (obj is IJsonParserCallbackReceiver receiver)
            {
                //触发转换开始回调
                receiver.OnToJsonStart();
            }

            if (reflection)
            {
                //反射转换

                if (TypeUtil.IsArrayOrList(obj))
                {
                    //数组或list
                    AppendJsonArray(type, obj, 0);
                }
                else if (TypeUtil.IsDictionary(obj))
                {
                    //字典
                    AppendJsonDict(type,obj, 0);
                }
                else
                {
                    //自定义类
                    Type realType = TypeUtil.GetType(obj);
                    AppendJsonObject(obj, realType, 0, !TypeUtil.TypeEquals(type,realType));
                }
            }
            else
            {
                Type realType = TypeUtil.GetType(obj);
                if (GenJsonCodes.ToJsonCodeFuncDict.TryGetValue(realType, out Action<object, int> action))
                {
                    //使用预生成代码转换
                    action(obj, 0);
                }
                else
                {
                    throw new Exception($"没有为{type}类型预生成的转换代码");
                }
            }

            string json = TextUtil.CachedSB.ToString();
            TextUtil.CachedSB.Clear();
            return json;
        }

        /// <summary>
        /// 追加json key文本
        /// </summary>
        public static void AppendJsonKey(string key, int depth)
        {
            AppendJsonValue(key,depth);
            TextUtil.Append(":");
        }

        /// <summary>
        /// 追加Json值文本
        /// </summary>
        public static void AppendJsonValue(Type type, object obj, int depth)
        {
            Type realType = TypeUtil.GetType(obj);
            if (ExtensionToJsonFuncDict.TryGetValue(realType, out Action<object> action))
            {
                //自定义转换Json文本方法
                action(obj);
                return;
            }

            //根据属性值的不同类型进行序列化
            if (TypeUtil.IsNumber(obj))
            {
                //数字
                TextUtil.Append(obj.ToString());
                return;
            }

            if (obj is string || obj is char)
            {
                //字符串
                AppendJsonValue(obj.ToString());
                return;
            }

            if (obj is bool)
            {
                //bool值
                bool b = (bool)obj;
                AppendJsonValue(b);
                return;
            }

            if (obj is Enum)
            {
                //枚举
                int enumInt = (int)obj;
                AppendJsonValue(enumInt);
                return;
            }

            if (TypeUtil.IsArrayOrList(obj))
            {
                //数组或List
                AppendJsonArray(type, obj, depth);
                return;
            }


            if (TypeUtil.IsDictionary(obj))
            {
                //字典
                AppendJsonDict(type, obj, depth);
                return;
            }

            //自定义类对象
            AppendJsonObject(obj, realType, depth, !TypeUtil.TypeEquals(type,realType));
        }
        
        /// <summary>
        /// 追加Json数据类对象文本
        /// </summary>
        private static void AppendJsonObject(object obj, Type realType, int depth, bool isPolymorphic = false)
        {
            
            if (!propertyInfoDict.ContainsKey(realType) && !fieldInfoDict.ContainsKey(realType))
            {
                //初始化反射信息
                AddToReflectionMap(realType);
            }

            //是否需要删除最后一个逗号
            bool needRemoveLastComma = false;
            
            TextUtil.AppendLine("{");

            //处理多态序列化
            if (isPolymorphic)
            {
                AppendJsonKey(RealTypeKey, depth + 1);
                AppendJsonValue(GetRealTypeJsonValue(realType));
                TextUtil.AppendLine(",");
                needRemoveLastComma = true;
            }

            //序列化属性
            foreach (KeyValuePair<RangeString, PropertyInfo> item in propertyInfoDict[realType])
            {
                object value = item.Value.GetValue(obj);
                if (TypeUtil.IsDefaultValue(value))
                {
                    //默认值跳过序列化
                    continue;
                }
                    
                AppendMember(item.Value.PropertyType,item.Value.Name,value,depth);
                needRemoveLastComma = true;
            }

            //序列化字段
            foreach (KeyValuePair<RangeString, FieldInfo> item in fieldInfoDict[realType])
            {
                object value = item.Value.GetValue(obj);
                if (TypeUtil.IsDefaultValue(value))
                {
                    //默认值跳过序列化
                    continue;
                }
                AppendMember(item.Value.FieldType,item.Value.Name,value,depth);
                needRemoveLastComma = true;
            }

            if (needRemoveLastComma)
            {
                //删除末尾多出来的逗号
                
                //需要删除的字符长度
                int needRemoveLength = 1;
                if (IsFormat)
                {
                    //开启了格式化序列化，需要额外删除一个换行符
                    needRemoveLength += TextUtil.NewLineLength;
                }

                //最后一个逗号的位置
                int lastCommaIndex = TextUtil.CachedSB.Length - needRemoveLength;
                
                TextUtil.CachedSB.Remove(lastCommaIndex, needRemoveLength);
            }
            
            TextUtil.AppendLine(string.Empty);
            TextUtil.Append("}",depth);
        }
        
        /// <summary>
        /// 追加json数组文本
        /// </summary>
        private static void AppendJsonArray(Type arrayType, object arrayObj, int depth)
        {
            TextUtil.AppendLine("[");
            Type elementType = TypeUtil.GetArrayOrListElementType(arrayType);
            if (arrayType.IsArray)
            {
                Array array = (Array)arrayObj;
                for (int i = 0; i < array.Length; i++)
                {
                    object element = array.GetValue(i);
                    TextUtil.AppendTab(depth + 1);
                    if (element == null)
                    {
                        TextUtil.Append("null");
                    }
                    else
                    {
                        AppendJsonValue(elementType, element, depth + 1);
                    }
                    if (i < array.Length-1)
                    {
                        TextUtil.AppendLine(",");
                    }
                 
                }
            }
            else
            {
                IList list = (IList)arrayObj;
                for (int i = 0; i < list.Count; i++)
                {
                    object element = list[i];
                    TextUtil.AppendTab(depth + 1);
                    if (element == null)
                    {
                        TextUtil.Append("null");
                    }
                    else
                    {
                        AppendJsonValue(elementType, element, depth + 1);
                    }

                    if (i < list.Count-1)
                    {
                        TextUtil.AppendLine(",");
                    }
                }
            }
            TextUtil.AppendLine(string.Empty);
            TextUtil.Append("]", depth);
        }

        /// <summary>
        /// 追加json字典文本
        /// </summary>
        private static void AppendJsonDict(Type dictType, object dictObj, int depth)
        {
            //字典
            IDictionary dict = (IDictionary)dictObj;
            IDictionaryEnumerator enumerator = dict.GetEnumerator();
            Type valueType = TypeUtil.GetDictValueType(dictType);
            TextUtil.AppendLine("{");
            int index = 0;
            while (enumerator.MoveNext())
            {
                AppendJsonKey(enumerator.Key.ToString(), depth + 1);
                
                if (enumerator.Value == null)
                {
                    TextUtil.Append("null");
                }
                else
                {
                    AppendJsonValue(valueType, enumerator.Value, depth + 1);
                }
                
                if (index < dict.Count-1)
                {
                    TextUtil.AppendLine(",");
                }

                index++;
            }

            TextUtil.AppendLine(string.Empty);
            TextUtil.Append("}", depth);
        }
        
        /// <summary>
        /// 追加字段/属性的json文本
        /// </summary>
        private static void AppendMember(Type memberType,string memberName,object value,int depth)
        {
            AppendJsonKey(memberName, depth + 1);
            AppendJsonValue(memberType, value, depth + 1);
            TextUtil.AppendLine(",");
        }
        

        //------------------------为内置基础类型提供append方法------------------------------------
        public static void AppendJsonValue(bool b, int depth = 0)
        {
            if (b == true)
            {
                TextUtil.Append("true", depth);
            }
            else
            {
                TextUtil.Append("false", depth);
            }
        }

        public static void AppendJsonValue(string s, int depth = 0)
        {
            TextUtil.Append("\"", depth);
            TextUtil.Append(s);
            TextUtil.Append("\"");
        }

        public static void AppendJsonValue(char c, int depth = 0)
        {
            TextUtil.Append("\"", depth);
            TextUtil.Append(c.ToString());
            TextUtil.Append("\"");
        }

        public static void AppendJsonValue(byte b, int depth = 0)
        {
            TextUtil.Append(b.ToString(), depth);
        }

        public static void AppendJsonValue(int i, int depth = 0)
        {
            TextUtil.Append(i.ToString(), depth);
        }

        public static void AppendJsonValue(long l, int depth = 0)
        {
            TextUtil.Append(l.ToString(), depth);
        }

        public static void AppendJsonValue(float f, int depth = 0)
        {
            TextUtil.Append(f.ToString(CultureInfo.InvariantCulture), depth);
        }

        public static void AppendJsonValue(double d, int depth = 0)
        {
            TextUtil.Append(d.ToString(CultureInfo.InvariantCulture), depth);
        }

        public static void AppendJsonValue(sbyte s, int depth = 0)
        {
            TextUtil.Append(s.ToString(), depth);
        }

        public static void AppendJsonValue(short s, int depth = 0)
        {
            TextUtil.Append(s.ToString(), depth);
        }

        public static void AppendJsonValue(uint ui, int depth = 0)
        {
            TextUtil.Append(ui.ToString(), depth);
        }

        public static void AppendJsonValue(ulong ul, int depth = 0)
        {
            TextUtil.Append(ul.ToString(), depth);
        }

        public static void AppendJsonValue(ushort us, int depth = 0)
        {
            TextUtil.Append(us.ToString(), depth);
        }

        public static void AppendJsonValue(decimal d, int depth = 0)
        {
            TextUtil.Append(d.ToString(CultureInfo.InvariantCulture), depth);
        }
    }
}