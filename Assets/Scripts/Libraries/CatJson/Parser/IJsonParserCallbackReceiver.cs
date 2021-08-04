using System.Collections;
using System.Collections.Generic;

namespace CatJson
{
    /// <summary>
    /// Json转换/解析回调接收接口
    /// </summary>
    public interface IJsonParserCallbackReceiver
    {
        /// <summary>
        /// Json转换开始回调
        /// </summary>
        void OnToJsonStart();

        /// <summary>
        /// json解析结束回调
        /// </summary>
        void OnParseJsonEnd();
    }
}

