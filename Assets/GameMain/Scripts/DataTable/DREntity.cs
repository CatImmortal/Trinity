using GameFramework.DataTable;
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace Trinity
{
    /// <summary>
    /// 实体表。
    /// </summary>
    public class DREntity : DataRowBase
    {


        public override int Id
        {
            get
            {
                throw new System.NotImplementedException();
            }
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
            //Id = int.Parse(text[index++]);
            index++;
            AssetName = text[index++];
        }

        private void AvoidJIT()
        {
            new Dictionary<int, DREntity>();
        }
    }
}
