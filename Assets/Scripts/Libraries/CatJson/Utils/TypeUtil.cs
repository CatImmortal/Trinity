using System;
using System.Collections;
using System.Collections.Generic;

#if FUCK_LUA
using ILRuntime.CLR.Utils;
using ILRuntime.Reflection;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
#endif

namespace CatJson
{
    /// <summary>
    /// 类型相关的工具类
    /// </summary>
    public static class TypeUtil
    {
                
#if FUCK_LUA
        public static ILRuntime.Runtime.Enviorment.AppDomain AppDomain;
#endif
        
        /// <summary>
        /// 检查Type,如果是ILRuntimeWrapperType需要返回其所包装的主工程Type
        /// </summary>
        public static Type CheckType(Type type)
        {
#if FUCK_LUA
            if (type is ILRuntimeWrapperType wt)
            {
                return wt.RealType;
            }
#endif
            return type;
        }

        /// <summary>
        /// 对比Type是否为同一类型的Type
        /// </summary>
        public static bool TypeEquals(Type t1, Type t2)
        {
#if FUCK_LUA
            t1 = CheckType(t1);
            t2 = CheckType(t2);
#endif
            return t1 == t2;
        }
        
        /// <summary>
        /// 获取obj的Type
        /// </summary>
        public static Type GetType(object obj)
        {
#if FUCK_LUA
            if (obj is ILTypeInstance ins)
            {
               return ins.Type.ReflectionType;
            }
            if (obj is CrossBindingAdaptorType cross)
            {
                return cross.ILInstance.Type.ReflectionType;
            }
#endif
            return obj.GetType();
        }
        
        /// <summary>
        /// 根据memberType和realTypeValue获取字段/属性的真实Type
        /// </summary>
        public static Type GetRealType(Type memberType, string realTypeValue)
        {
#if FUCK_LUA
            if (memberType is ILRuntimeType ilrtType)
            {
                return AppDomain.GetType(realTypeValue).ReflectionType;
            }
#endif
            
            return Type.GetType(realTypeValue);
        }

        /// <summary>
        /// 创建type的实例
        /// </summary>
        public static object CreateInstance(Type type)
        {
#if FUCK_LUA
            if (type is ILRuntimeType ilrtType)
            {
                return ilrtType.ILType.Instantiate();
            }
#endif
            return Activator.CreateInstance(type);
        }
        

        
        /// <summary>
        /// type是否为内置基础类型 (string char bool 数字)
        /// </summary>
        public static bool IsBaseType(Type type)
        {
            return type == typeof(string) || type == typeof(char) || type == typeof(bool) || IsNumberType(type);
        }
        
        /// <summary>
        /// obj是否为数字
        /// </summary>
        public static bool IsNumber(object obj)
        {
            return obj is byte || obj is int || obj is long || obj is float || obj is double
                   ||obj is uint||obj is ulong ||obj is ushort ||obj is short ||obj is decimal 
                   ||obj is sbyte ;
        }
        
        /// <summary>
        /// type是否为数字类型
        /// </summary>
        public static bool IsNumberType(Type type)
        {
            return type == typeof(byte) || type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double)
            ||type == typeof(uint)||type == typeof(ulong)||type == typeof(ushort)||type == typeof(short) ||type == typeof(decimal)
            ||type == typeof(sbyte);
        }


        /// <summary>
        /// obj是否为数组或List
        /// </summary>
        public static bool IsArrayOrList(object obj)
        {
            return obj is Array || obj is IList;
        }

        /// <summary>
        /// type是否为数组或List类型
        /// </summary>
        public static bool IsArrayOrListType(Type type)
        {
            return type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>));
        }
        
        /// <summary>
        /// 获取数组或List的元素类型
        /// </summary>
        public static Type GetArrayOrListElementType(Type arrayType)
        {
            Type elementType;
            if (arrayType.IsArray)
            {
                //数组
#if FUCK_LUA
                if (arrayType is ILRuntimeWrapperType wt)
                {
                    elementType = wt.CLRType.ElementType.ReflectionType;
                }
                else
#endif
                {
                    elementType = arrayType.GetElementType();
                }
            }
            else
            {
                //List
#if FUCK_LUA
                if (arrayType is ILRuntimeWrapperType wt)
                {
                    elementType = wt.CLRType.GenericArguments[0].Value.ReflectionType;
                }
                else
#endif
                {
                    elementType = arrayType.GenericTypeArguments[0];

                }
            }

            return elementType;
        }

        /// <summary>
        /// obj是否为字典
        /// </summary>
        public static bool IsDictionary(object obj)
        {
            return obj is IDictionary;
        }

        /// <summary>
        /// type是否为字典类型
        /// </summary>
        public static bool IsDictionaryType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        /// <summary>
        /// 获取字典value的类型
        /// </summary>
        public static Type GetDictValueType(Type dictType)
        {                       
            Type valueType;
#if FUCK_LUA
            if (dictType is ILRuntimeWrapperType wt)
            {
                valueType = wt.CLRType.GenericArguments[1].Value.ReflectionType;
            }
            else         
#endif
            {
                valueType = dictType.GetGenericArguments()[1];
            }
            return valueType;
        }


        /// <summary>
        /// value是否为内置基础类型的默认值(null 0 false)
        /// </summary>
        public static bool IsDefaultValue(object value)
        {
            if (!(value is System.ValueType))
            {
                return value == default;
            }

            if (value is byte b)
            {
                return b == default;
            }

            if (value is int i)
            {
                return i == default;
            }
            if (value is long l)
            {
                return l == default;
            }
            if (value is float f)
            {
                return Math.Abs(f - default(float)) < 1e-6f;
            }
            if (value is double d)
            {
                return Math.Abs(d - default(double)) < 1e-15;
            }

            if (value is bool boolean)
            {
                return boolean == default;
            }
            
            if (value is sbyte sb)
            {
                return sb == default;
            }
            if (value is short s)
            {
                return s == default;
            }
            if (value is uint ui)
            {
                return ui == default;
            }
            if (value is ulong ul)
            {
                return ul == default;
            }
            if (value is ushort us)
            {
                return us == default;
            }
            if (value is decimal de)
            {
                return de == default;
            }
            
            return false;
        }



    }
}

