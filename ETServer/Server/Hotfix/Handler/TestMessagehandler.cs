using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    class TestMessagehandler : AMHandler<TestMessage>
    {
        protected override void Run(Session session, TestMessage message)
        {
            Log.Debug(message.Info);
        }
    }
}
