namespace Trinity
{
    /// <summary>
    /// 版本信息（对应服务器上的version.txt）
    /// </summary>
    public class VersionInfo
    {
        public bool ForceGameUpdate
        {
            get;
            set;
        }

        public string LatestGameVersion
        {
            get;
            set;
        }

        public int InternalGameVersion
        {
            get;
            set;
        }

        public int InternalResourceVersion
        {
            get;
            set;
        }

        public string GameUpdateUrl
        {
            get;
            set;
        }

        public int VersionListLength
        {
            get;
            set;
        }

        public int VersionListHashCode
        {
            get;
            set;
        }

        public int VersionListZipLength
        {
            get;
            set;
        }

        public int VersionListZipHashCode
        {
            get;
            set;
        }
    }
}
