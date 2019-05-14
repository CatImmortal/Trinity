using System;
using ETModel;
using Trinity.Hotfix;
namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    class RPCHotfixHandler : AMRpcHandler<RPCHotfixRequest, RPCHotfixResponse>
    {
        protected override void Run(Session session, RPCHotfixRequest message, Action<RPCHotfixResponse> reply)
        {
            Log.Info(message.Info);
            reply(new RPCHotfixResponse() { Info = "Hello Client" });
        }
    }
}
