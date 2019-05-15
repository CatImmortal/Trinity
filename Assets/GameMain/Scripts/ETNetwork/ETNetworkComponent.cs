using ETModel;
using GameFramework;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Trinity
{
    /// <summary>
    /// ET网络组件
    /// </summary>
    public class ETNetworkComponent : GameFrameworkComponent
    {
        /// <summary>
        /// 服务器地址
        /// </summary>
        [SerializeField]
        public string ServerIP;

        /// <summary>
        /// 网络组件
        /// </summary>
        private NetOuterComponent m_NetOuter;

        private void Start()
        {
            
            try
            {
                //异步操作回调到主线程
                SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
                Game.EventSystem.Add(DLLType.Model, typeof(Game).Assembly);

                //添加ET组件到Game.Scene

                ///网络组件
                m_NetOuter = Game.Scene.AddComponent<NetOuterComponent>();

                //消息识别码组件
                Game.Scene.AddComponent<OpcodeTypeComponent>();

                //消息分发组件
                Game.Scene.AddComponent<MessageDispatcherComponent>();
            }
            catch (System.Exception e)
            {
                throw new GameFrameworkException("初始化ET网络组件时出现异常："+e);
            }

        }

        private void Update()
        {
            OneThreadSynchronizationContext.Instance.Update();
            Game.EventSystem.Update();
        }


        private void OnDestroy()
        {
            Game.Close();
        }

        /// <summary>
        /// 创建会话
        /// </summary>
        public Session CreateSession(IPEndPoint iPEndPoint)
        {
            return m_NetOuter.Create(iPEndPoint);
        }

        /// <summary>
        /// 创建会话
        /// </summary>
        public Session CreateSession(string address)
        {
            return m_NetOuter.Create(address);
        }

        /// <summary>
        /// 添加会话组件
        /// </summary>
        public void AddSessionComponent(Session session)
        {
            Game.Scene.AddComponent<SessionComponent>().Session = session;
        }

        /// <summary>
        /// 使用会话组件保存的会话发送网络消息
        /// </summary>
        public void Send(IMessage message)
        {
            SessionComponent.Instance.Session.Send(message);
        }
    }
}


