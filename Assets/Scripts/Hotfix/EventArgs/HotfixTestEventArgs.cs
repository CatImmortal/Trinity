using GameFramework.Event;

namespace Trinity.Hotfix
{
    public class HotfixTestEventArgs : HotfixGameEventArgs
    {

        public static readonly int EventId = typeof(HotfixTestEventArgs).GetHashCode();


        public override int Id
        {
            get
            {
                return EventId;
            }
        }

        public string Str
        {
            get;
            private set;
        }

        public HotfixTestEventArgs Fill(string str)
        {
            Str = str;
            return this;
        }

        public override void Clear()
        {
            Str = string.Empty;
        }
    }

}
