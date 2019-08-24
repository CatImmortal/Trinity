using System;
using System.Collections;
using System.Collections.Generic;
using ETHotfix;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace Trinity.Hotfix
{
    public class ProcedureHotfixTest : ProcedureBase
    {


        protected internal override void OnEnter(IFsm procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Debug.Log("进入了热更新测试流程");
        }

        protected internal override async void OnUpdate(IFsm procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (Input.GetKeyDown(KeyCode.A))
            {
                Session session = HotfixGameEntry.ETNetwork.CreateHotfixSession();
                session.Send(new HotfixTestMessage() { Info = "6666" });

                HotfixRpcResponse response = (HotfixRpcResponse)await session.Call(new HotfixRpcRequest() { Info = "Hello Server" });
                Debug.Log(response.Info);
                session.Dispose();
            }
        }

        

    }
}

