using System;
using System.Collections.Generic;
using System.Text;
using ETModel;
namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    class G2C_TestHotfixHandler : AMHandler<G2C_TestHotfixMessage>
    {
        protected override void Run(Session session, G2C_TestHotfixMessage message)
        {
            Log.Info("收到了客户端发来的热更新层消息：" + message.Info);
        }
    }
}
