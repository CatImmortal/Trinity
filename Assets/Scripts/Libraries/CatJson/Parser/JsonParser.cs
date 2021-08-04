using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

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
        private static Dictionary<Type, Func<object>> extensionParseFuncDict = new Dictionary<Type, Func<object>>();

        /// <summary>
        /// 扩展类型与其对应的转换Json文本方法
        /// </summary>
        private static Dictionary<Type, Action<object>> extensionToJsonFuncDict = new Dictionary<Type, Action<object>>();

        public static Dictionary<Type, HashSet<string>> IgnoreSet = new Dictionary<Type, HashSet<string>>();

        /// <summary>
        /// 解析Json对象的通用流程
        /// </summary>
        public static void ParseJsonObjectProcedure(object userdata1,object userdata2,Action<object,object,RangeString, TokenType> action)
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
                TokenType nextTokenType = Lexer.LookNextTokenType();

                action(userdata1,userdata2,key, nextTokenType);

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
        /// 将type的公有实例字段和属性都放入字典中缓存
        /// </summary>
        private static void AddToReflectionMap(Type type)
        {
            IgnoreSet.TryGetValue(type, out HashSet<string> set);

            PropertyInfo[] pis = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            Dictionary<RangeString, PropertyInfo> dict1 = null;
            if (pis.Length > 0)
            {
                dict1 = new Dictionary<RangeString, PropertyInfo>(pis.Length);
                for (int i = 0; i < pis.Length; i++)
                {
                    PropertyInfo pi = pis[i];

                    if (pi.GetCustomAttribute<JsonIgnoreAttribute>() != null)
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
                
            }
            propertyInfoDict.Add(type, dict1);

            FieldInfo[] fis = type.GetFields(BindingFlags.Instance | BindingFlags.Public);
            Dictionary<RangeString, FieldInfo> dict2 = null;
            if (fis.Length > 0)
            {
                dict2 = new Dictionary<RangeString, FieldInfo>(fis.Length);
                for (int i = 0; i < fis.Length; i++)
                {
                    FieldInfo fi = fis[i];
                    if (fi.GetCustomAttribute<JsonIgnoreAttribute>() != null)
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

            }
            fieldInfoDict.Add(type, dict2);
        }


    }

}
