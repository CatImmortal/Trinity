using System;
using ETModel;

namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    class HotfixRpcHandler : AMRpcHandler<HotfixRpcRequest, HotfixRpcResponse>
    {

        protected override void Run(Session session, HotfixRpcRequest message, Action<HotfixRpcResponse> reply)
        {
            Log.Info(message.Info);
            reply(new HotfixRpcResponse() { Info = "Hello Client" });
        }
    }
}
