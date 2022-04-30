using System.Collections.Generic;
using System;

namespace CatJson
{
    /// <summary>
    /// Json解析器
    /// </summary>
    public static class JsonParser
    {
#if FUCK_LUA
        static JsonParser()
        {
            if (TypeUtil.AppDomain == null)
            {
                throw new Exception("请先调用CatJson.ILRuntimeHelper.RegisterILRuntimeCLRRedirection(appDomain)进行CatJson重定向");
            }
        }
#endif
        
        /// <summary>
        /// Json词法分析器
        /// </summary>
        public static readonly JsonLexer Lexer = new JsonLexer();

        /// <summary>
        /// 序列化时是否开启格式化
        /// </summary>
        public static bool IsFormat { get; set; } = true;

        private static readonly NullFormatter nullFormatter = new NullFormatter();
        private static readonly ArrayFormatter arrayFormatter = new ArrayFormatter();
        private static readonly DefaultFormatter defaultFormatter = new DefaultFormatter();
        private static readonly PolymorphicFormatter polymorphicFormatter = new PolymorphicFormatter();
        
        /// <summary>
        /// Json格式化器字典
        /// </summary>
        private static readonly Dictionary<Type, IJsonFormatter> formatterDict = new Dictionary<Type, IJsonFormatter>()
        {
            //基元类型
            {typeof(bool), new BooleanFormatter()},
            {typeof(int), new Int32Formatter()},
            {typeof(float), new SingleFormatter()},
            {typeof(double), new DoubleFormatter()},
            {typeof(string), new StringFormatter()},
            
            //容器类型
            {typeof(List<>), new ListFormatter()},
            {typeof(Dictionary<,>), new DictionaryFormatter()},
            
            //特殊类型
            {typeof(JsonObject), new JsonObjectFormatter()},
            {typeof(JsonValue), new JsonValueFormatter()},
        };

        /// <summary>
        /// 注册Json格式化器
        /// </summary>
        public static void RegisterFormatter(Type type, IJsonFormatter formatter)
        {
            formatterDict[type] = formatter;
        }


        /// <summary>
        /// 将指定类型的对象序列化为Json文本
        /// </summary>
        public static string ToJson<T>(T obj)
        {
            InternalToJson<T>(obj);

            string json = TextUtil.CachedSB.ToString();
            TextUtil.CachedSB.Clear();

            return json;
        }

        /// <summary>
        /// 将指定类型的对象序列化为Json文本
        /// </summary>
        public static string ToJson(object obj, Type type)
        {
            InternalToJson(obj, type);

            string json = TextUtil.CachedSB.ToString();
            TextUtil.CachedSB.Clear();

            return json;
        }
        
        /// <summary>
        /// 将Json文本反序列化为指定类型的对象
        /// </summary>
        public static T ParseJson<T>(string json)
        {
            Lexer.SetJsonText(json);
            return InternalParseJson<T>();
        }

        /// <summary>
        /// 将Json文本反序列化为指定类型的对象
        /// </summary>
        public static object ParseJson(string json, Type type)
        {
            Lexer.SetJsonText(json);
            return InternalParseJson(type);
        }

        /// <summary>
        /// 将指定类型的对象序列化为Json文本
        /// </summary>
        internal static void InternalToJson<T>(T obj, int depth = 0)
        {
            InternalToJson(obj, typeof(T),null, depth);
        }
        
        /// <summary>
        /// 将Json文本反序列化为指定类型的对象
        /// </summary>
        internal static T InternalParseJson<T>()
        {
            return (T) InternalParseJson(typeof(T));
        }

        /// <summary>
        /// 将指定类型的对象序列化为Json文本
        /// </summary>
        internal static void InternalToJson(object obj, Type type, Type realType = null, int depth = 0,bool checkPolymorphic = true)
        {
            if (obj is null)
            {
                nullFormatter.ToJson(null,type,null, depth);
                return;
            }

            if (realType == null)
            {
                realType = TypeUtil.GetType(obj,type);
            }
            
            if (checkPolymorphic && !TypeUtil.TypeEquals(type,realType))
            {
                //开启了多态序列化检测
                //只要定义类型和真实类型不一致，就要进行多态序列化
                polymorphicFormatter.ToJson(obj,type,realType,depth);
                return;;
            }
            
            if (formatterDict.TryGetValue(realType, out IJsonFormatter formatter))
            {
                formatter.ToJson(obj,type,realType, depth);
                return;
            }

            if (realType.IsGenericType && formatterDict.TryGetValue(realType.GetGenericTypeDefinition(), out formatter))
            {
                //特殊处理泛型类型
                formatter.ToJson(obj,type,realType,depth);
                return;
            }
            
            if (obj is Array array)
            {
                //特殊处理数组
                arrayFormatter.ToJson(array,type,realType, depth);
                return;
            }
            
            //使用处理自定义类的formatter
            defaultFormatter.ToJson(obj,type,realType,depth);
        }
        
        /// <summary>
        /// 将Json文本反序列化为指定类型的对象
        /// </summary>
        internal static object InternalParseJson(Type type,Type realType = null,bool checkPolymorphic = true)
        {
            if (Lexer.LookNextTokenType() == TokenType.Null)
            {
                return nullFormatter.ParseJson(type,null);
            }

            object result;

            if (realType == null && !ParserHelper.TryParseRealType(type,out realType))
            {
                //未传入realType并且读取不到realType，就把type作为realType使用
                //这里不能直接赋值type，因为type有可能是一个包装了主工程类型的ILRuntimeWrapperType
                //直接赋值type会导致无法从formatterDict拿到正确的formatter从而进入到defaultFormatter的处理中
                //realType = type;  
                realType = TypeUtil.CheckType(type);
            }

            if (checkPolymorphic && !TypeUtil.TypeEquals(type,realType))
            {
                //开启了多态检查并且type和realType不一致
                //进行多态处理
                result = polymorphicFormatter.ParseJson(type, realType);
                return result;
            }

            if (formatterDict.TryGetValue(realType, out IJsonFormatter formatter))
            {
               result = formatter.ParseJson(type,realType);
               return result;
            }
            
            if (realType.IsGenericType &&  formatterDict.TryGetValue(realType.GetGenericTypeDefinition(), out formatter))
            {
                //特殊处理泛型类型
                result = formatter.ParseJson(type,realType);
                return result;
            }
            
            if (realType.IsArray)
            {
                //特殊处理数组
                result = arrayFormatter.ParseJson(type,realType);
                return result;
 
            }
            
            //使用处理自定义类的formatter
            result = defaultFormatter.ParseJson(type,realType);
            return result;
        }

    }

}
