using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    class TestRPCHandler : AMRpcHandler<RPCTestRequest, RPCTestResponse>
    {
        protected override void Run(Session session, RPCTestRequest message, Action<RPCTestResponse> reply)
        {
            Log.Info(message.Info);
            reply(new RPCTestResponse() { Info = "Hello Client" });
        }
    }
}
