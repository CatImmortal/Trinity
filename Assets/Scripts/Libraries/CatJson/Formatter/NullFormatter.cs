using System;

namespace CatJson
{
    /// <summary>
    /// null值的Json格式化器
    /// </summary>
    public class NullFormatter : IJsonFormatter
    {
        /// <inheritdoc />
        public void ToJson(object value, Type type, Type realType, int depth)
        {
            TextUtil.Append("null");
        }

        /// <inheritdoc />
        public object ParseJson(Type type, Type realType)
        {
            JsonParser.Lexer.GetNextTokenByType(TokenType.Null);
            return null;
        }
    }
}