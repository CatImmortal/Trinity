using System;

namespace CatJson
{
    /// <summary>
    /// JsonValue类型的Json格式化器
    /// </summary>
    public class JsonValueFormatter : BaseJsonFormatter<JsonValue>
    {
        /// <inheritdoc />
        public override void ToJson(JsonValue value, Type type, Type realType, int depth)
        {
            switch (value.Type)
            {
                case ValueType.Null:
                    TextUtil.Append("null");
                    break;
                case ValueType.Boolean:
                    JsonParser.InternalToJson(value.Boolean);
                    break;
                case ValueType.Number:
                    JsonParser.InternalToJson(value.Number);
                    break;
                case ValueType.String:
                    JsonParser.InternalToJson(value.Str);
                    break;
                case ValueType.Array:
                    JsonParser.InternalToJson(value.Array, depth + 1);
                    break;
                case ValueType.Object:
                    JsonParser.InternalToJson(value.Obj, depth + 1);
                    break;
            }
        }

        /// <inheritdoc />
        public override JsonValue ParseJson(Type type, Type realType)
        {
            //这里只能look不能get，get交给各类型的formatter去进行
            TokenType nextTokenType = JsonParser.Lexer.LookNextTokenType();
            
            switch (nextTokenType)
            {
                case TokenType.True:
                case TokenType.False:
                    return new JsonValue(JsonParser.InternalParseJson<bool>());
                
                case TokenType.Number:
                    return new JsonValue(JsonParser.InternalParseJson<double>());
                
                case TokenType.String:
                    return new JsonValue(JsonParser.InternalParseJson<string>());
                
                case TokenType.LeftBracket:
                    return new JsonValue(JsonParser.InternalParseJson<JsonValue[]>());
                
                case TokenType.LeftBrace:
                    return new JsonValue(JsonParser.InternalParseJson<JsonObject>());
                
                default:
                    throw new Exception("JsonValue解析失败，tokenType == " + nextTokenType);
            }

        }
    }
}