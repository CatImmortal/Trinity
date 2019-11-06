using System;

namespace Trinity
{
    /// <summary>
    /// 热更新层实体数据
    /// </summary>
    [Serializable]
    public class HotfixEntityData : EntityData
    {
        /// <summary>
        /// 对应的热更新层实体逻辑类名
        /// </summary>
        public string HotfixEntityName
        {
            get;
            private set;
        }

        /// <summary>
        /// 要传递给热更新层实体的实体数据
        /// </summary>
        public object UserData
        {
            get;
            private set;
        }

        public HotfixEntityData()
        {

        }

        /// <summary>
        /// 填充实体数据
        /// </summary>
        public HotfixEntityData Fill(int id, int typeId, string hotfixEntityName, object userData)
        {
            Fill(id, typeId);
            HotfixEntityName = hotfixEntityName;
            UserData = userData;
            return this;
        }

        public override void Clear()
        {
            base.Clear();

            HotfixEntityName = null;
            UserData = null;
        }
    }
}

