using ETHotfix;
using UnityEngine;
using Trinity.Hotfix;
using Log = UnityGameFramework.Runtime.Log;

namespace Trinity.Hotfix
{
    /// <summary>
    /// 热更新层游戏入口
    /// </summary>
    public class HotfixGameEntry
    {

        /// <summary>
        /// 有限状态机
        /// </summary>
        public static FsmManager Fsm
        {
            get;
            private set;
        }

        /// <summary>
        /// 流程
        /// </summary>
        public static ProcedureManager Procedure
        {
            get;
            private set;
        }

        /// <summary>
        /// 事件
        /// </summary>
        public static EventManager Event
        {
            get;
            private set;
        }

        /// <summary>
        /// ET网络
        /// </summary>
        public static ETNetworkManager ETNetwork
        {
            get;
            private set;
        }


        public void Start()
        {
            Log.Info("热更新层启动!");

            Fsm = new FsmManager();
            Procedure = new ProcedureManager();
            Event = new EventManager();
            ETNetwork = new ETNetworkManager();

            //初始化流程管理器并开始流程
            //TODO:可修改为使用反射获取到所有流程然后注册
            Procedure.Initialize(Fsm
                , new ProcedureHotfixEntry()
                , new ProcedureChangeScene()
                );

            Procedure.StartProcedure<ProcedureHotfixEntry>();

            //初始化ET网络
            ETNetwork.Init();
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            Fsm.Update(elapseSeconds, realElapseSeconds);
            Event.Update(elapseSeconds, realElapseSeconds);
        }

        public void ShutDown()
        {
            Procedure.Shutdown();
            Fsm.Shutdown();
            Event.Shutdown();
            ETNetwork.Shutdown();
        }
    }
}

