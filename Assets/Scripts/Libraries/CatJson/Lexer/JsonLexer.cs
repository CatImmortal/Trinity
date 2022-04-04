using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
namespace CatJson
{
    /// <summary>
    /// Json词法分析器
    /// </summary>
    public class JsonLexer
    {
        private string json;
        private int curIndex;

        private bool hasNextTokenCache;
        private TokenType nextTokenType;
        private RangeString nextToken;

        private static StringBuilder cachedSB = new StringBuilder();


        /// <summary>
        /// 设置Json文本
        /// </summary>
        public void SetJsonText(string json)
        {
            this.json = json;
            curIndex = 0;
            hasNextTokenCache = false;
        }

        public int GetCurIndex()
        {
            return curIndex;
        }

        /// <summary>
        /// 查看下一个token的类型
        /// </summary>
        public TokenType LookNextTokenType()
        {
            if (hasNextTokenCache)
            {
                //有缓存直接返回缓存
                return nextTokenType;
            }

            //没有就get一下
            nextToken = GetNextToken(out nextTokenType);
            hasNextTokenCache = true;
            return nextTokenType;
        }

        /// <summary>
        /// 获取下一个指定类型的token
        /// </summary>
        public RangeString GetNextTokenByType(TokenType type)
        {
            RangeString token = GetNextToken(out TokenType resultType);
            if (type != resultType)
            {
                throw new Exception($"NextTokenOfType调用失败，需求{type}但获取到的是{resultType}");
            }
            return token;
        }

        /// <summary>
        /// 获取下一个token
        /// </summary>
        public RangeString GetNextToken(out TokenType type)
        {
            type = default;

            if (hasNextTokenCache)
            {
                //有缓存下一个token的信息 直接返回
                type = nextTokenType;

                hasNextTokenCache = false;

                return nextToken;
            }

            //跳过空白字符
            SkipWhiteSpace();

            if (curIndex >= json.Length)
            {
                //文本结束
                type = TokenType.Eof;
                return default;
            }

            //扫描字面量 分隔符
            switch (json[curIndex])
            {
                case 'n':
                    type = TokenType.Null;
                    ScanLiteral("null");
                    return default;
                case 't':
                    type = TokenType.True;
                    ScanLiteral("true");
                    return default;
                case 'f':
                    type = TokenType.False;
                    ScanLiteral("false");
                    return default;
                case '[':
                    type = TokenType.LeftBracket;
                    Next();
                    return default;
                case ']':
                    type = TokenType.RightBracket;
                    Next();
                    return default;
                case '{':
                    type = TokenType.LeftBrace;
                    Next();
                    return default;
                case '}':
                    type = TokenType.RightBrace;
                    Next();
                    return default;
                case ':':
                    type = TokenType.Colon;
                    Next();
                    return default;
                case ',':
                    type = TokenType.Comma;
                    Next();
                    return default;
            }

            //扫描数字
            if (char.IsDigit(json[curIndex]) || json[curIndex] == '-')
            {
                string str = ScanNumber();
                type = TokenType.Number;
                return new RangeString(str);
            }

            //扫描字符串
            if (json[curIndex] == '"')
            {
                RangeString rs = ScanString();
                type = TokenType.String;
                return rs;
            }

            throw new Exception("json解析失败，当前字符:" + json[curIndex]);
        }

        /// <summary>
        /// 移动CurIndex
        /// </summary>
        private void Next(int n = 1)
        {
            curIndex += n;
        }


        /// <summary>
        /// 跳过空白字符
        /// </summary>
        private void SkipWhiteSpace()
        {
            if (curIndex >= json.Length)
            {
                return;
            }

            char c = json[curIndex];
            while (!(curIndex >= json.Length) && (c == ' ' || c == '\t' || c == '\n' || c == '\r'))
            {
                Next();
                c = json[curIndex];
            }
        }

        /// <summary>
        /// 剩余json字符串是否以prefix开头
        /// </summary>
        private bool IsPrefix(string prefix)
        {
            int tempCurIndex = curIndex;
            for (int i = 0; i < prefix.Length; i++, tempCurIndex++)
            {
                if (json[tempCurIndex] != prefix[i])
                {
                    return false;
                }
            }
            return true;
        }



        /// <summary>
        /// 扫描字面量 null true false
        /// </summary>
        private void ScanLiteral(string prefix)
        {
            if (IsPrefix(prefix))
            {

                Next(prefix.Length);
                return;
            }

            throw new Exception($"Json语法错误,{prefix}解析失败");
        }

        /// <summary>
        /// 扫描数字
        /// </summary>
        private string ScanNumber()
        {
            //第一个字符是0-9或者-
            cachedSB.Append(json[curIndex]);
            Next();

            while (
                !(curIndex >= json.Length)&&
                (
                char.IsDigit(json[curIndex]) || json[curIndex] == '.' || json[curIndex] == '+'|| json[curIndex] == '-'|| json[curIndex] == 'e'|| json[curIndex] == 'E')
                )
            {
                cachedSB.Append(json[curIndex]);
                Next();
            }

            string result = cachedSB.ToString();
            cachedSB.Clear();

            return result;
        }

        /// <summary>
        /// 扫描字符串
        /// </summary>
        private RangeString ScanString()
        {

            // 起始字符是 " 要跳过
            Next();

            int startIndex = curIndex;

            while (!(curIndex >= json.Length) & json[curIndex] != '"')
            {
               
                Next();
            }
            // 字符串中有转义\" 的需要继续
            bool isNeedBack = false;
            if (json[curIndex-1] == '\\')
            {
                int index = 2;
                while (curIndex-index!=0 && json[curIndex-index]=='\\' )
                {
                    index++;
                }
                if (index%2==0)
                {
                    ScanString();
                    isNeedBack = true;
                }
            }

            if (isNeedBack)
            {
                Next(-1);
            }
            int endIndex = curIndex - 1;

            if (curIndex >= json.Length)
            {
                throw new Exception("字符串扫描失败，不是以双引号结尾的");
            }
            else
            {
                // 末尾也是 " 也要跳过
                Next();
            }

            RangeString rs = new RangeString(json, startIndex, endIndex);

            return rs;
        }

        /// <summary>
        /// 设置当前索引
        /// </summary>
        public void SetCurIndex(int index = 0, bool hasNext = false)
        {
            curIndex = index;
            hasNextTokenCache = hasNext;
        }
    }

}
