using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    class HotfixTestMessagehandler : AMHandler<HotfixTestMessage>
    {
        protected override void Run(Session session, HotfixTestMessage message)
        {
            Log.Debug(message.Info);
        }
    }
}
