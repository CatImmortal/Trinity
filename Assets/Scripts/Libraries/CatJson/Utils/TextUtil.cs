using System.Collections;
using System.Collections.Generic;
using System.Text;
using System;
using System.Reflection;
namespace CatJson
{
    /// <summary>
    /// 文本相关的工具类
    /// </summary>
    public static class TextUtil
    {
        public static StringBuilder CachedSB = new StringBuilder();

        /// <summary>
        /// 当前平台的换行符长度
        /// </summary>
        public static int NewLineLength => Environment.NewLine.Length;


        public static void AppendTab(int tabNum)
        {
            if (!JsonParser.IsFormat)
            {
                return;
            }
            for (int i = 0; i < tabNum; i++)
            {
                CachedSB.Append("\t");
            }
        }

        public static void Append(string str,int tabNum = 0)
        {
            if (tabNum > 0 && JsonParser.IsFormat)
            {
                AppendTab(tabNum);
            }
           
            CachedSB.Append(str);
        }

        public static void AppendLine(string str, int tabNum = 0)
        {
            if (tabNum > 0 && JsonParser.IsFormat)
            {
                AppendTab(tabNum);
            }

            if (JsonParser.IsFormat)
            {
                CachedSB.AppendLine(str);
            }
            else
            {
                CachedSB.Append(str);
            }
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
    

    }

}
