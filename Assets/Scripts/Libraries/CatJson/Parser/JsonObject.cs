using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// json对象
    /// </summary>
    public class JsonObject
    {
        private Dictionary<string, JsonValue> valueDict;

        public JsonValue this[string key]
        {
            get
            {
                if (valueDict == null)
                {
                    return null;
                }

                return valueDict[key];
            }

            set
            {
                if (valueDict == null)
                {
                    valueDict = new Dictionary<string, JsonValue>();
                }
                valueDict[key] = value;
            }
        }

        public override string ToString()
        {
            if (valueDict == null)
            {
                return "{} ";
            }
            string str = "{";
            int count = 0;
            foreach (KeyValuePair<string, JsonValue> item in valueDict)
            {
                count++;
                str += "\"" + item.Key + "\"" + " : " + item.Value;
                if (count < valueDict.Count)
                {
                    str += ", ";
                }
            }
            str += "} ";
            return str;
        }
  
        public void ToJson(int depth)
        {
            Util.AppendLine("{");

            if (valueDict != null)
            {
                foreach (KeyValuePair<string, JsonValue> item in valueDict)
                {

                    Util.Append("\"", depth + 1);
                    Util.Append(item.Key);
                    Util.Append("\"");

                    Util.Append(":");

                    item.Value.ToJson(depth + 1);

                    Util.AppendLine(",");
                }

                if (valueDict.Count > 0)
                {
                    //删掉最后的 , 字符
                    Util.CachedSB.Remove(Util.CachedSB.Length - 3, 1);
                }
            }

            Util.Append("}", depth);
        }
    }
}

