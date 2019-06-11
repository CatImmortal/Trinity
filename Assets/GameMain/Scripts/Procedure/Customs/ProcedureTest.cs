using GameFramework.Fsm;
using GameFramework.Procedure;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;
using ETModel;
using GameFramework;
namespace Trinity
{
    public class ProcedureTest : ProcedureBase
    {
        protected override void OnEnter(IFsm<IProcedureManager> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Debug.Log("进入了测试流程");
        }


        protected override async void OnUpdate(IFsm<IProcedureManager> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (Input.GetKeyDown(KeyCode.A))
            {
                Session session = GameEntry.ETNetwork.CreateSession(GameEntry.ETNetwork.ServerIP);
                session.Send(new TestMessage() { Info = "6666" });

                TestRpcResponse response = (TestRpcResponse)await session.Call(new TestRpcRequest() { Info = "Hello Server" });
                Debug.Log(response.Info);
                session.Dispose();
            }

        }
    }
}

