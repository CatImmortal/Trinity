using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// Json值
    /// </summary>
    public class JsonValue
    {
        

        public ValueType Type;

        public bool Boolean;
        public double Number;
        public string Str;
        public JsonValue[] Array;
        public JsonObject Obj;

        public override string ToString()
        {
            switch (Type)
            {
                case ValueType.Null:
                    return "null";
                case ValueType.Boolean:
                    return Boolean.ToString();
                case ValueType.Number:
                    return Number.ToString();
                case ValueType.String:
                    return "\"" + Str + "\"";
                case ValueType.Array:
                    string str = "[";
                    for (int i = 0; i < Array.Length; i++)
                    {
                        str += Array[i];

                        if (i != Array.Length - 1)
                        {
                            str += " ,";
                        };
                    }
                    str += "] ";
                    return str;
                case ValueType.Object:
                    return Obj.ToString();
            }

            return "";
        }
        public void ToJson(int depth)
        {
            switch (Type)
            {
                case ValueType.Null:
                    TextUtil.Append("null");
                    break;
                case ValueType.Boolean:
                    if (Boolean == true)
                    {
                        TextUtil.Append("true");
                    }
                    else
                    {
                        TextUtil.Append("false");
                    }
                    break;
                case ValueType.Number:
                    TextUtil.Append(Number.ToString());
                    break;
                case ValueType.String:
                    TextUtil.Append("\"");
                    TextUtil.Append(Str);
                    TextUtil.Append("\"");
                    break;
                case ValueType.Array:
                    TextUtil.AppendLine("[");
                    for (int i = 0; i < Array.Length; i++)
                    {
                        TextUtil.AppendTab(depth + 1);
                        JsonValue jv = Array[i];
                        jv.ToJson(depth + 1);
                        if (i<Array.Length-1)
                        {
                            TextUtil.AppendLine(",");
                        }
                    }
                    TextUtil.AppendLine(string.Empty);
                    TextUtil.Append("]",depth);
                    break;
                case ValueType.Object:
                    Obj.ToJson(depth);
                    break;
            }
        }
    }

}
