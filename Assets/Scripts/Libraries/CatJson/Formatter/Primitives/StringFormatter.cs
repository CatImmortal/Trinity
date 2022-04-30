using System;

namespace CatJson
{
    /// <summary>
    /// 字符串类型的Json格式化器
    /// </summary>
    public class StringFormatter : BaseJsonFormatter<string>
    {
        /// <inheritdoc />
        public override void ToJson(string value, Type type, Type realType, int depth)
        {
            TextUtil.Append($"\"{value}\"");
        }

        /// <inheritdoc />
        public override string ParseJson(Type type, Type realType)
        {
            RangeString rs = JsonParser.Lexer.GetNextTokenByType(TokenType.String);
            return rs.ToString();
        }
    }
}