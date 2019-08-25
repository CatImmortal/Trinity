using GameFramework;
using System;
using ETHotfix;

namespace Trinity.Hotfix
{
    public class ETNetworkManager
    {

        public void Init()
        {
            try
            {
                Game.Scene.AddComponent<OpcodeTypeComponent>();
                Game.Scene.AddComponent<MessageDispatcherComponent>();
            }
            catch (Exception e)
            {
                throw new GameFrameworkException("初始化热更新层ET网络组件时出现异常：", e);
            }
        }

        public void Shutdown()
        {
            Game.Close();
        }

        /// <summary>
        /// 创建热更新层会话
        /// </summary>
        public Session CreateHotfixSession()
        {
            return CreateHotfixSession(GameEntry.ETNetwork.CreateSession(GameEntry.ETNetwork.ServerIP));
        }

        /// <summary>
        /// 创建热更新层会话
        /// </summary>
        public Session CreateHotfixSession(ETModel.Session session)
        {
            return ComponentFactory.Create<Session, ETModel.Session>(session);
        }

        /// <summary>
        /// 添加热更新层会话组件
        /// </summary>
        public void AddSessionComponent(Session session)
        {
            Game.Scene.AddComponent<SessionComponent>().Session = session;
        }

        /// <summary>
        /// 使用热更新层会话组件保存的会话发送网络消息
        /// </summary>
        public void Send(IMessage message)
        {
            SessionComponent.Instance.Session.Send(message);
        }

        /// <summary>
        /// 使用热更新层会话组件保存的会话发送网络消息
        /// </summary>
        public ETModel.ETTask<IResponse> Call(IRequest request)
        {
            return SessionComponent.Instance.Session.Call(request);
        }

    }
}

