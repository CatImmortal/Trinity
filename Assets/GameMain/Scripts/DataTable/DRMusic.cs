using GameFramework;
using GameFramework.DataTable;
using System.Collections.Generic;
using System.IO;

namespace Trinity
{
    /// <summary>
    /// 音乐配置表。
    /// </summary>
    public class DRMusic : IDataRow
    {
        /// <summary>
        /// 音乐编号。
        /// </summary>
        public int Id
        {
            get;
            protected set;
        }

        /// <summary>
        /// 资源名称。
        /// </summary>
        public string AssetName
        {
            get;
            private set;
        }

        public void ParseDataRow(string dataRowText)
        {
            string[] text = DataTableExtension.SplitDataRow(dataRowText);
            int index = 0;
            index++;
            Id = int.Parse(text[index++]);
            index++;
            AssetName = text[index++];
        }

        public bool ParseDataRow(GameFrameworkSegment<string> dataRowSegment)
        {
            throw new System.NotImplementedException();
        }

        public bool ParseDataRow(GameFrameworkSegment<byte[]> dataRowSegment)
        {
            throw new System.NotImplementedException();
        }

        public bool ParseDataRow(GameFrameworkSegment<Stream> dataRowSegment)
        {
            throw new System.NotImplementedException();
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DRMusic>();
        }
    }
}
