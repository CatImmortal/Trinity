using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    public class TestHandler : AMHandler<TestMessage>
    {
        protected override void Run(Session session, TestMessage message)
        {
            Log.Info(message.Info);
        }
    }
}
