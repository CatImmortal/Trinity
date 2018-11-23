using ETModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace ETHotfix
{
    [MessageHandler(AppType.AllServer)]
    public class HotfixTestHandler : AMHandler<HotfixTestMessage>
    {
        protected override void Run(Session session, HotfixTestMessage message)
        {
            Log.Info("收到了客户端发来的消息：" + message.Text);
        }
    }
}
