using System;
using ETModel;

namespace Trinity.Hotfix
{

    /// <summary>
    /// 消息处理器接口
    /// </summary>
    public interface IMHandler
    {
        void Handle(ETModel.Session session, object message);
        Type GetMessageType();
    }

}