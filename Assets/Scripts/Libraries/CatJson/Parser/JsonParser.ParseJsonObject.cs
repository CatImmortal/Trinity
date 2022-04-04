using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatJson
{
    public static partial class JsonParser
    {
        /// <summary>
        /// 解析json文本为Json对象
        /// </summary>
        public static JsonObject ParseJson(string json)
        {
            Lexer.SetJsonText(json);

            return ParseJsonObject();
        }

        /// <summary>
        /// 解析json值
        /// </summary>
        public static JsonValue ParseJsonValue(TokenType nextTokenType)
        {
            JsonValue value = new JsonValue();

            switch (nextTokenType)
            {

                case TokenType.Null:
                    Lexer.GetNextToken(out _);
                    value.Type = ValueType.Null;
                    break;
                case TokenType.True:
                    Lexer.GetNextToken(out _);
                    value.Type = ValueType.Boolean;
                    value.Boolean = true;
                    break;
                case TokenType.False:
                    Lexer.GetNextToken(out _);
                    value.Type = ValueType.Boolean;
                    value.Boolean = false;
                    break;
                case TokenType.Number:
                    RangeString token = Lexer.GetNextToken(out _);
                    value.Type = ValueType.Number;
                    value.Number = double.Parse(token.ToString());
                    break;
                case TokenType.String:
                    token = Lexer.GetNextToken(out _);
                    value.Type = ValueType.String;
                    value.Str = token.ToString();
                    break;
                case TokenType.LeftBracket:
                    value.Type = ValueType.Array;
                    value.Array = ParseJsonArray();
                    break;
                case TokenType.LeftBrace:
                    value.Type = ValueType.Object;
                    value.Obj = ParseJsonObject();
                    break;
                default:
                    throw new Exception("JsonValue解析失败，tokenType == " + nextTokenType);
            }

            return value;
        }

        /// <summary>
        /// 解析json对象
        /// </summary>
        private static JsonObject ParseJsonObject()
        {
            JsonObject obj = new JsonObject();

            ParseJsonObjectProcedure(obj, null,false, (userdata1, userdata2,isIntKey, key, nextTokenType) => {
                JsonValue value = ParseJsonValue(nextTokenType);
                JsonObject jo = (JsonObject)userdata1;
                jo[key.ToString()] = value;
            });

            return obj;

        }

        /// <summary>
        /// 解析Json数组
        /// </summary>
        private static JsonValue[] ParseJsonArray()
        {
            List<JsonValue> list = new List<JsonValue>();

            ParseJsonArrayProcedure(list, null, (userdata1, _, nextTokenType) => {
                JsonValue value = ParseJsonValue(nextTokenType);
                List<JsonValue> valueList = (List<JsonValue>)userdata1;
                valueList.Add(value);
            });

            return list.ToArray();
        }
    }
}

