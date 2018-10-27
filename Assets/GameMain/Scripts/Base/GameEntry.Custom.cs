namespace Trinity
{
    /// <summary>
    /// 游戏入口。
    /// </summary>
    public partial class GameEntry
    {
        public static BuiltinDataComponent BuiltinData
        {
            get;
            private set;
        }

        public static ETNetworkComponent ETNetwork
        {
            get;
            private set;
        }

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
