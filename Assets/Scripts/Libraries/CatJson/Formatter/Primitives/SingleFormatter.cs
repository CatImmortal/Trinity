using System;

namespace CatJson
{
    /// <summary>
    /// float类型的Json格式化器
    /// </summary>
    public class SingleFormatter : BaseJsonFormatter<float>
    {
        /// <inheritdoc />
        public override void ToJson(float value, Type type, Type realType, int depth)
        {
            TextUtil.Append(value.ToString());
        }

        /// <inheritdoc />
        public override float ParseJson(Type type, Type realType)
        {
            RangeString rs = JsonParser.Lexer.GetNextTokenByType(TokenType.Number);
            return float.Parse(rs.ToString());
        }
    }
}