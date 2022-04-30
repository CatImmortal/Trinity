using System;

namespace CatJson
{
    /// <summary>
    /// bool类型的Json格式化器
    /// </summary>
    public class BooleanFormatter : BaseJsonFormatter<bool>
    {
        /// <inheritdoc />
        public override void ToJson(bool value, Type type, Type realType, int depth)
        {
            string json = "true";
            if (!value)
            {
                json = "false";
            }
            
            TextUtil.Append(json);
        }

        /// <inheritdoc />
        public override bool ParseJson(Type type, Type realType)
        {
            JsonParser.Lexer.GetNextToken(out TokenType nextTokenType);
            return nextTokenType == TokenType.True;
        }
    }
}