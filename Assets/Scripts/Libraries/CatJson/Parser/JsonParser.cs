
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#if FUCK_LUA
using ILRuntime.CLR.Method;
using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Utils;
using ILRuntime.Reflection;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
#endif
namespace CatJson
{
    /// <summary>
    /// json解析器
    /// </summary>
    public static partial class JsonParser
    {
        /// <summary>
        /// Json词法分析器
        /// </summary>
        public static JsonLexer Lexer = new JsonLexer();

        /// <summary>
        /// 类型与其对应的属性信息
        /// </summary>
        private static Dictionary<Type, Dictionary<RangeString, PropertyInfo>> propertyInfoDict = new Dictionary<Type, Dictionary<RangeString, PropertyInfo>>();
        
        /// <summary>
        /// 类型与其对应的字段信息
        /// </summary>
        private static Dictionary<Type, Dictionary<RangeString, FieldInfo>> fieldInfoDict = new Dictionary<Type, Dictionary<RangeString, FieldInfo>>();

        /// <summary>
        /// 扩展类型与其对应的解析方法
        /// </summary>
        public static Dictionary<Type, Func<object>> ExtensionParseFuncDict = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// 扩展类型与其对应的转换Json文本方法
        /// </summary>
        public static Dictionary<Type, Action<object>> ExtensionToJsonFuncDict = new Dictionary<Type, Action<object>>();

        /// <summary>
        /// 需要忽略的类型字段/属性名称
        /// </summary>
        public static Dictionary<Type, HashSet<string>> IgnoreSet = new Dictionary<Type, HashSet<string>>();

        /// <summary>
        /// 序列化时是否开启格式化
        /// </summary>
        public static bool IsFormat { get; set; } = true;

        /// <summary>
        /// 真实类型key
        /// </summary>
        private const string RealTypeKey = "<>RealType";


        /// <summary>
        /// 将type的公有实例字段和属性都放入字典中缓存
        /// </summary>
        private static void AddToReflectionMap(Type type)
        {
            IgnoreSet.TryGetValue(type, out HashSet<string> set);

            PropertyInfo[] pis = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            Dictionary<RangeString, PropertyInfo>  dict1 = new Dictionary<RangeString, PropertyInfo>(pis.Length);
            for (int i = 0; i < pis.Length; i++)
            {
                PropertyInfo pi = pis[i];

                if (Attribute.IsDefined(pi, typeof(JsonIgnoreAttribute)))
                {
                    //需要忽略
                    continue;
                }

                if (set != null && set.Contains(pi.Name))
                {
                    //需要忽略
                    continue;
                }

                if (pi.SetMethod != null && pi.GetMethod != null && pi.Name != "Item")
                {
                    //属性必须同时具有get set 并且不能是索引器item
                    dict1.Add(new RangeString(pi.Name), pi);
                }
                    
            }
            propertyInfoDict.Add(type, dict1);

            FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            Dictionary<RangeString, FieldInfo> dict2 = new Dictionary<RangeString, FieldInfo>(fis.Length);
            for (int i = 0; i < fis.Length; i++)
            {
                FieldInfo fi = fis[i];

                if (Attribute.IsDefined(fi,typeof(JsonIgnoreAttribute)))
                {
                    //需要忽略
                    continue;
                }

                if (set != null && set.Contains(fi.Name))
                {
                    //需要忽略
                    continue;
                }

                dict2.Add(new RangeString(fi.Name), fi);
            }
            fieldInfoDict.Add(type, dict2);
        }
        
        /// <summary>
        /// 解析Json对象的通用流程
        /// </summary>
        public static void ParseJsonObjectProcedure(object userdata1,object userdata2,bool isIntKey,Action<object,object,bool,RangeString, TokenType> action)
        {

            //跳过 {
            Lexer.GetNextTokenByType(TokenType.LeftBrace);

            while (Lexer.LookNextTokenType() != TokenType.RightBrace)
            {
                //提取key
                RangeString key = Lexer.GetNextTokenByType(TokenType.String);

                //跳过 :
                Lexer.GetNextTokenByType(TokenType.Colon);

                //提取value
                //array和json obj需要完整的[]和{}，所以只能look
                //此时curIndex停留在value后的第一个字符上
                TokenType nextTokenType = Lexer.LookNextTokenType();

                action(userdata1,userdata2,isIntKey,key, nextTokenType);

                //有逗号就跳过逗号
                if (Lexer.LookNextTokenType() == TokenType.Comma)
                {
                    Lexer.GetNextTokenByType(TokenType.Comma);

                    if (Lexer.LookNextTokenType() == TokenType.RightBracket)
                    {
                        throw new Exception("Json对象不能以逗号结尾");
                    }
                }
                else
                {
                    //没有逗号就说明结束了
                    break;
                }

            }

            //跳过 }
            Lexer.GetNextTokenByType(TokenType.RightBrace);
        }

        /// <summary>
        /// 解析Json数组的通用流程
        /// </summary>
        public static void ParseJsonArrayProcedure(object userdata1,object userdata2, Action<object,object,TokenType> action)
        {
            //跳过[
            Lexer.GetNextTokenByType(TokenType.LeftBracket);

            while (Lexer.LookNextTokenType() != TokenType.RightBracket)
            {
                //提取value
                //array和json obj需要完整的[]和{}，所以只能look
                TokenType nextTokenType = Lexer.LookNextTokenType();

                action(userdata1,userdata2,nextTokenType);

                //有逗号就跳过
                if (Lexer.LookNextTokenType() == TokenType.Comma)
                {
                    Lexer.GetNextTokenByType(TokenType.Comma);

                    if (Lexer.LookNextTokenType() == TokenType.RightBracket)
                    {
                        throw new Exception("数组不能以逗号结尾");
                    }
                }
                else
                {
                    //没有逗号就说明结束了
                    break;
                }
            }

            //跳过]
            Lexer.GetNextTokenByType(TokenType.RightBracket);
        }
        

        /// <summary>
        /// 获取用于多态序列化的真实类型的json value字符串
        /// </summary>
        private static string GetRealTypeJsonValue(Type type)
        {
#if FUCK_LUA
            if (type is ILRuntimeType ilrtType)
            {
                 return $"{ilrtType.FullName}";
            }
#endif
            return $"{type.FullName},{type.Assembly.GetName().Name}";
        }
        

        
#if FUCK_LUA
         public unsafe static void RegisterILRuntimeCLRRedirection(ILRuntime.Runtime.Enviorment.AppDomain appdomain)
         {
            TypeUtil.AppDomain = appdomain;
            foreach (MethodInfo mi in typeof(JsonParser).GetMethods())
            {
                if (mi.Name == "ParseJson" && mi.IsGenericMethodDefinition)
                {
                    appdomain.RegisterCLRMethodRedirection(mi, RedirectionParseJson);
                }
            }
        }

        public unsafe static StackObject* RedirectionParseJson(ILIntepreter intp, StackObject* esp, IList<object> mStack, CLRMethod method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(esp, 1);
            bool reflection = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(esp, 2);
            string json = (string)typeof(string).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, mStack));

            intp.Free(ptr_of_this_method);

            Type type = method.GenericArguments[0].ReflectionType;

            object result_of_this_method = ParseJson(json, type);

            return ILIntepreter.PushObject(__ret, mStack, result_of_this_method);
        }
#endif
    }
}