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
                    Util.Append("null");
                    break;
                case ValueType.Boolean:
                    if (Boolean == true)
                    {
                        Util.Append("true");
                    }
                    else
                    {
                        Util.Append("false");
                    }
                    break;
                case ValueType.Number:
                    Util.Append(Number.ToString());
                    break;
                case ValueType.String:
                    Util.Append("\"");
                    Util.Append(Str);
                    Util.Append("\"");
                    break;
                case ValueType.Array:
                    Util.AppendLine("[");
                    for (int i = 0; i < Array.Length; i++)
                    {
                        Util.AppendTab(depth + 1);
                        JsonValue jv = Array[i];
                        jv.ToJson(depth + 1);
                        Util.AppendLine(",");
                    }
                    if (Array.Length > 0)
                    {
                        //删掉最后的 , 字符
                        Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
                    }
                    Util.Append("]",depth);
                    break;
                case ValueType.Object:
                    Obj.ToJson(depth);
                    break;
                default:
                    break;
            }
        }
    }

}
