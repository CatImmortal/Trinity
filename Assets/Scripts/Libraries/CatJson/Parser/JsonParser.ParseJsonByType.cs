using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

#if FUCK_LUA
using ILRuntime.CLR.Utils;
using ILRuntime.Reflection;
using ILRuntime.Runtime.Intepreter;
#endif

namespace CatJson
{
    public static partial class JsonParser
    {
        /// <summary>
        /// 解析json文本为指定类型的对象实例
        /// </summary>
        public static T ParseJson<T>(string json, bool reflection = true)
        {
            return (T)ParseJson(json, typeof(T), reflection);
        }

        /// <summary>
        /// 解析json文本为指定类型的对象实例
        /// </summary>
        public static object ParseJson(string json, Type type, bool reflection = true)
        {
            Lexer.SetJsonText(json);

            object result = null;

            if (reflection)
            {
                //使用反射解析

                if (TypeUtil.IsArrayOrListType(type) || TypeUtil.IsDictionaryType(type))
                {
                    //数组或list或字典
                    TokenType nextTokenType = Lexer.LookNextTokenType();
                    result = ParseJsonValueByType(nextTokenType, type);

                }
                else
                {
                    //数据类
                    result = ParseJsonObjectByType(type);
                }
            }
            else
            {
                if (GenJsonCodes.ParseJsonCodeFuncDict.TryGetValue(type, out Func<object> func))
                {
                    //使用预生成代码解析
                    result = func();
                }
                else
                {
                    throw new Exception($"没有为{type}类型预生成的解析代码");
                }  
            }

            if (result != null && result is IJsonParserCallbackReceiver receiver)
            {
                //触发解析结束回调
                receiver.OnParseJsonEnd();
            }

            return result;
        }


        /// <summary>
        /// 解析json值为指定类型的实例值
        /// </summary>
        public static object ParseJsonValueByType(TokenType nextTokenType, Type type)
        {
            if (ExtensionParseFuncDict.TryGetValue(type, out Func<object> func))
            {
                //自定义解析
                return func();
            }

            switch (nextTokenType)
            {
                case TokenType.Null:
                    Lexer.GetNextToken(out _);
                    if (!type.IsValueType)
                    {
                        return null;
                    }
                    break;

                case TokenType.True:
                case TokenType.False:
                    Lexer.GetNextToken(out _);
                    if (type == typeof(bool))
                    {
                        return nextTokenType == TokenType.True;
                    }
                    break;

                case TokenType.Number:
                    RangeString token = Lexer.GetNextToken(out _);
                    string str = token.ToString();
                    if (type == typeof(byte))
                    {
                        return byte.Parse(str);
                    }
                    if (type == typeof(int))
                    {
                        return int.Parse(str);
                    }
                    if (type == typeof(long))
                    {
                        return long.Parse(str);
                    }
                    if (type == typeof(float))
                    {
                        return float.Parse(str);
                    }
                    if (type == typeof(double))
                    {
                        return double.Parse(str);
                    }
                    
                    if (type == typeof(sbyte))
                    {
                        return sbyte.Parse(str);
                    }
                    if (type == typeof(short))
                    {
                        return short.Parse(str);
                    }
                    if (type == typeof(uint))
                    {
                        return uint.Parse(str);
                    }
                    if (type == typeof(ulong))
                    {
                        return ulong.Parse(str);
                    }
                    if (type == typeof(ushort))
                    {
                        return ushort.Parse(str);
                    }
                    if (type == typeof(decimal))
                    {
                        return decimal.Parse(str);
                    }
                    
#if FUCK_LUA
                    if (type is ILRuntimeType ilrtType && ilrtType.ILType.IsEnum)
                    {
                        //热更层枚举 
                        return int.Parse(str);
                    }
#endif

                    if (type.IsEnum)
                    {
                        //枚举
                        int enumInt = int.Parse(str);
                        return Enum.ToObject(type, enumInt);
                    }
                    break;

                case TokenType.String:
                    token = Lexer.GetNextToken(out _);
                    if (type == typeof(string))
                    {
                        return token.ToString();
                    }
                    if (type == typeof(char))
                    {
                        return char.Parse(token.ToString());
                    }
                    break;

                case TokenType.LeftBracket:

                    if (TypeUtil.IsArrayOrListType(type))
                    {
                        Type elementType = TypeUtil.GetArrayOrListElementType(type);
                        return ParseJsonArrayByType(type,elementType);
                    }

                    break;

                case TokenType.LeftBrace:

                    if (TypeUtil.IsDictionaryType(type))
                    {
                        //字典

                        Type valueType = TypeUtil.GetDictValueType(type);
                        return ParseJsonObjectByDict(type, valueType);
                    }

                    //类对象
                    return ParseJsonObjectByType(type);

            }

            throw new Exception("ParseJsonValueByType调用失败，tokenType == " + nextTokenType + ",type == " + type.FullName);
        }

        /// <summary>
        /// 解析json对象为指定类型的数据类实例
        /// </summary>
        public static object ParseJsonObjectByType(Type type)
        {
            //保证了传入的type参数一定是真实类型，，所以可以直接使用它来创建对象实例以及使用它的反射信息
            object obj = TypeUtil.CreateInstance(type);

            if (!propertyInfoDict.ContainsKey(type) && !fieldInfoDict.ContainsKey(type))
            {
                //初始化反射信息
                AddToReflectionMap(type);
            }

            ParseJsonObjectProcedure(obj, type, false, (userdata1, userdata2, isIntKey, key, nextTokenType) =>
            {
                object parentObj = userdata1;
                Type parentType = (Type) userdata2;

                propertyInfoDict.TryGetValue(parentType, out Dictionary<RangeString, PropertyInfo> dict1);
                if (dict1 != null && dict1.TryGetValue(key, out PropertyInfo pi))
                {
                    //先尝试获取名为key的属性
                    Type realType = TryParseRealType(pi.PropertyType);
                    object value = ParseJsonValueByType(nextTokenType,realType);
                    pi.SetValue(parentObj, value);
                }
                else
                {
                    //属性没有 再试试字段
                    fieldInfoDict.TryGetValue(parentType, out Dictionary<RangeString, FieldInfo> dict2);
                    if (dict2 != null && dict2.TryGetValue(key, out FieldInfo fi))
                    {
                        Type realType = TryParseRealType(fi.FieldType);
                        object value = ParseJsonValueByType(nextTokenType, realType);
                        fi.SetValue(userdata1, value);
                    }
                    else
                    {
                        //这个json key既不是数据类的字段也不是属性，跳过
                        ParseJsonValue(nextTokenType);
                    }
                }
            });

            return obj;

        }
        
        /// <summary>
        /// 解析Json数组为指定类型的Array或List
        /// </summary>
        private static object ParseJsonArrayByType(Type arrayType,Type elementType)
        {
            IList list;
            if (arrayType.IsArray)
            {
                //数组
                list = new List<object>();
            }
            else
            {
                //List<T>
                list = (IList)Activator.CreateInstance(arrayType);
            }

            ParseJsonArrayProcedure(list, elementType, (userdata1, userdata2, nextTokenType) =>
            {
                
                object value = ParseJsonValueByType(nextTokenType, TryParseRealType((Type)userdata2));
                ((IList)userdata1).Add(value);
            });

            //返回List<T>
            if (!arrayType.IsArray)
            {
                return list;
            }

            //返回数组
            Array array = Array.CreateInstance(elementType, list.Count);
            for (int i = 0; i < list.Count; i++)
            {

                object element = list[i];
                array.SetValue(element, i);
            }

            return array;



        }

        /// <summary>
        /// 解析json对象为字典，key为string或int类型
        /// </summary>
        private static object ParseJsonObjectByDict(Type dictType, Type valueType)
        {
            IDictionary dict = (IDictionary)Activator.CreateInstance(dictType);
            Type keyType = dictType.GetGenericArguments()[0];
            ParseJsonObjectProcedure(dict, valueType,keyType == typeof(int), (userdata1, userdata2,isIntKey, key, nextTokenType) => {
                object value = ParseJsonValueByType(nextTokenType, TryParseRealType((Type)userdata2));
                if (!isIntKey)
                {
                    ((IDictionary)userdata1).Add(key.ToString(), value);
                }
                else
                {
                    //处理字典key为int的情况
                    ((IDictionary)userdata1).Add(int.Parse(key.ToString()), value);
                }
               
            });

            return dict;
        }


        /// <summary>
        /// 尝试解析真实类型
        /// </summary>
        private static Type TryParseRealType(Type memberType)
        {
            Type realType = memberType;
            
            if (Lexer.LookNextTokenType() == TokenType.LeftBrace)
            {
                int curIndex = Lexer.GetCurIndex(); //记下当前lexer的index，是在{后的第一个字符上
                Lexer.GetNextTokenByType(TokenType.LeftBrace); // {
                
                RangeString rs = Lexer.GetNextToken(out TokenType tokenType);
                if (tokenType == TokenType.String && rs.Equals(new RangeString(RealTypeKey))) //"<>RealType"
                {
                    Lexer.GetNextTokenByType(TokenType.Colon); // :
                    
                    rs = Lexer.GetNextTokenByType(TokenType.String); //RealType Value
                    realType = TypeUtil.GetRealType(memberType, rs.ToString());  //获取真实类型
                }

                Lexer.SetCurIndex(curIndex - 1); //回退到前一个{的位置上，并将缓存置空，因为被look过所以需要-1
            }

            return realType;
        }

    }
}

