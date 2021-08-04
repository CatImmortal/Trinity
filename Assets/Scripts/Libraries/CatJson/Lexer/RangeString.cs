using System;


namespace CatJson
{
    /// <summary>
    /// 范围字符串
    /// 表示在Source字符串中，从StartIndex到EndIndex范围的字符构成的字符串
    /// </summary>
    public struct RangeString : IEquatable<RangeString>
    {
        /// <summary>
        /// 源字符串
        /// </summary>
        private string source;

        /// <summary>
        /// 开始索引
        /// </summary>
        private int startIndex;

        /// <summary>
        /// 结束索引
        /// </summary>
        private int endIndex;

        /// <summary>
        /// 哈希码
        /// </summary>
        private int hashCode;

        public RangeString(string source) : this(source,0,source.Length - 1)
        {
        }

        public RangeString(string source, int startIndex, int endIndex)
        {
            this.source = source;
            this.startIndex = startIndex;
            this.endIndex = endIndex;
            hashCode = 0;
        }

        public bool Equals(RangeString other)
        {
            
            bool isSourceNullOrEmpty = string.IsNullOrEmpty(source);
            bool isOtherNullOrEmpty = string.IsNullOrEmpty(other.source);

            if (isSourceNullOrEmpty && isOtherNullOrEmpty)
            {
                return true;
            }

            if (isSourceNullOrEmpty || isOtherNullOrEmpty)
            {
                return false;
            }
            int length = endIndex - startIndex + 1;
            int otherLength = other.endIndex - other.startIndex + 1;
            if (length != otherLength)
            {
                return false;
            }

            for (int i = startIndex, j = other.startIndex; i <= endIndex; i++, j++)
            {
                if (source[i] != other.source[j])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            if (hashCode == 0 && !string.IsNullOrEmpty(source))
            {
                for (int i = startIndex; i <= endIndex; i++)
                {
                    hashCode = 31 * hashCode + source[i];
                }
            }

            return hashCode;
        }

        public override string ToString()
        {
            if (endIndex - startIndex + 1 == 0)
            {
                //长度为0 处理空字符串的情况
                return string.Empty;
            }

            if (startIndex == 0 && endIndex == source.Length - 1)
            {
                //处理表示整个source范围的情况
                return source;
            }

            for (int i = startIndex; i <= endIndex; i++)
            {
                char c = source[i];

                if (c != '\\')
                {
                    //普通字符
                    Util.CachedSB.Append(source[i]);
                    continue;
                }

                //处理转义字符

                if (i == endIndex)
                {
                    throw new Exception("处理转义字符失败，\\后没有剩余字符");
                }

                //检测\后的下一个字符
                i++;
                c = source[i];
                switch (c)
                {
                    case '"':
                        Util.CachedSB.Append('\"');
                        break;
                    case '\\':
                        Util.CachedSB.Append('\\');
                        break;
                    case '/':
                        Util.CachedSB.Append('/');
                        break;
                    case 'b':
                        Util.CachedSB.Append('\b');
                        break;
                    case 'f':
                        Util.CachedSB.Append('\f');
                        break;
                    case 'n':
                        Util.CachedSB.Append('\n');
                        break;
                    case 'r':
                        Util.CachedSB.Append('\r');
                        break;
                    case 't':
                        Util.CachedSB.Append('\t');
                        break;
                    case 'u':
                        //unicode字符
                        char codePoint = Util.GetUnicodeCodePoint(source[i + 1], source[i + 2], source[i + 3], source[i + 4]);
                        Util.CachedSB.Append(codePoint);
                        i += 4;
                        break;
                    default:
                        throw new Exception("处理转义字符失败，\\后的字符不在可转义范围内");
                }



            }

            string str = Util.CachedSB.ToString();
            Util.CachedSB.Clear();

            return str;
        }
    
       
    }
}
