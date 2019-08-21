namespace Trinity
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry
    {
        /// <summary>
        /// 自定义数据组件
        /// </summary>
        public static BuiltinDataComponent BuiltinData
        {
            get;
            private set;
        }

        /// <summary>
        /// ET网络组件
        /// </summary>
        public static ETNetworkComponent ETNetwork
        {
            get;
            private set;
        }

        /// <summary>
        /// ILRuntime组件
        /// </summary>
        public static ILRuntimeComponent ILRuntime
        {
            get;
            private set;
        }

        private static void InitCustomComponents()
        {
            BuiltinData = UnityGameFramework.Runtime.GameEntry.GetComponent<BuiltinDataComponent>();
            ETNetwork = UnityGameFramework.Runtime.GameEntry.GetComponent<ETNetworkComponent>();
            ILRuntime = UnityGameFramework.Runtime.GameEntry.GetComponent<ILRuntimeComponent>();
        }
    }
}
