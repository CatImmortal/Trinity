using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Reflection;
namespace CatJson
{
    public static class Util
    {
        public static StringBuilder CachedSB = new StringBuilder();

        public static void AppendTab(int tabNum)
        {
            for (int i = 0; i < tabNum; i++)
            {
                CachedSB.Append("\t");
            }
        }

        public static void Append(string str,int tabNum = 0)
        {
            if (tabNum > 0)
            {
                AppendTab(tabNum);
            }
           
            CachedSB.Append(str);
        }

        public static void AppendLine(string str, int tabNum = 0)
        {
            if (tabNum > 0)
            {
                AppendTab(tabNum);
            }

            CachedSB.AppendLine(str);
        }

        /// <summary>
        /// 是否为内置基础类型 (string char bool 数字)
        /// </summary>
        public static bool IsBaseType(Type type)
        {
            return type == typeof(string) || type == typeof(char) || type == typeof(bool) || IsNumber(type);
        }


        /// <summary>
        /// type是否为数字类型（byte int long float double)
        /// </summary>
        public static bool IsNumber(Type type)
        {
            return type == typeof(byte) || type == typeof(int) || type == typeof(long) || type == typeof(float) || type == typeof(double);
        }

        /// <summary>
        /// type是否为数组或List<T>类型
        /// </summary>
        public static bool IsArrayOrList(Type type)
        {
            return type.IsArray || (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>));
        }

        /// <summary>
        /// type是否为字典类型
        /// </summary>
        public static bool IsDictionary(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Dictionary<,>);
        }

        /// <summary>
        /// 获取数组和List的元素类型
        /// </summary>
        public static Type GetArrayElementType(Type type)
        {
            if (type.IsArray)
            {
                return type.GetElementType();
            }

            return type.GetGenericArguments()[0];
        }

        /// <summary>
        /// 获取4个字符代表的unicode码点
        /// </summary>
        public static char GetUnicodeCodePoint(char c1,char c2,char c3,char c4)
        {
            int temp = UnicodeChar2Int(c1) * 0x1000 + UnicodeChar2Int(c2) *0x100 + UnicodeChar2Int(c3)*0x10 + UnicodeChar2Int(c4);
            return (char)temp;
        }

        private static int UnicodeChar2Int(char c)
        {
            //0-9
            if (char.IsDigit(c))
            {
                return c - '0';
            }
            
            //A-F
            if (c >= 65 && c <= 70)
            {
                return c - 'A' + 10;
            }

            //a-f
            if (c >= 97 && c <= 102)
            {
                return c - 'a' + 10;
            }

            throw new Exception("Char2Int调用失败，当前字符为：" + c);
        }
    
        /// <summary>
        /// value是否为内置基础类型的默认值(null 0 false)
        /// </summary>
        public static bool IsDefaultValue(Type type,object value)
        {
            if (!type.IsValueType)
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
                return f == default;
            }
            if (value is double d)
            {
                return d == default;
            }

            if (value is bool boolean)
            {
                return boolean == default;
            }



            return false;

        }
    }

}
