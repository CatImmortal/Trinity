using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    class C2R_RPCTestHandler : AMRpcHandler<C2R_RPCTest, R2C_RPCTest>
    {
        protected override void Run(Session session, C2R_RPCTest message, Action<R2C_RPCTest> reply)
        {
            Log.Info("收到了客户端的RPC消息：" + message.Text);
            reply(new R2C_RPCTest() { Text = "Hello Client" });
        }
    }
}
