using GameFramework.Fsm;
using GameFramework.Procedure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Trinity
{
    public class ProcedureTest : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("进入了测试流程");



        }

        protected override async void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (Input.GetKeyDown(KeyCode.A))
            {
                ETModel.Session session = GameEntry.ETNetwork.CreateSession(GameEntry.ETNetwork.ServerIP);

                session.Send(new ETModel.TestMessage() { Info = "6666"});

                ETModel.RPCTestResponse response = (ETModel.RPCTestResponse)await session.Call(new ETModel.RPCTestRequest() { Info = "Hello Server" });
                Log.Info(response.Info);
                session.Dispose();
            }
        }

    }
}

