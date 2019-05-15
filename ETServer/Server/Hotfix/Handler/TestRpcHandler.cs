using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    class TestRpcHandler : AMRpcHandler<TestRpcRequest, TestRpcResponse>
    {
        protected override void Run(Session session, TestRpcRequest message, Action<TestRpcResponse> reply)
        {
            Log.Info(message.Info);
            reply(new TestRpcResponse() { Info = "Hello Client" });
        }
    }
}
