using System;

namespace CatJson
{
    /// <summary>
    /// double类型的Json格式化器
    /// </summary>
    public class DoubleFormatter : BaseJsonFormatter<double>
    {
        /// <inheritdoc />
        public override void ToJson(double value, Type type, Type realType, int depth)
        {
            TextUtil.Append(value.ToString());
        }

        /// <inheritdoc />
        public override double ParseJson(Type type, Type realType)
        {
            RangeString rs = JsonParser.Lexer.GetNextTokenByType(TokenType.Number);
            return double.Parse(rs.ToString());
        }
    }
}