using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    public static partial class JsonParser
    {
        /// <summary>
        /// 将Json对象转换为Json文本
        /// </summary>
        public static string ToJson(JsonObject jo)
        {
            jo.ToJson(0);
            string json = TextUtil.CachedSB.ToString();
            TextUtil.CachedSB.Clear();
            return json;
        }
    }

}
