using ETModel;
using Trinity.Hotfix;

namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    public class HotfixTestHandler : AMHandler<HotfixTestMessage>
    {
        protected override void Run(Session session, HotfixTestMessage message)
        {
            Log.Info(message.Info);
        }
    }
}
