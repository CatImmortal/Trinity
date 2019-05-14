using System;
using System.Collections;
using System.Collections.Generic;
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

            Log.Info("进入了热更新测试流程");

        }

        protected internal async override void OnUpdate(IFsm procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (Input.GetKeyDown(KeyCode.A))
            {
                Session session = ETNetwork.CreateHotfixSession(GameEntry.ETNetwork.CreateSession(GameEntry.ETNetwork.ServerIP));
                RPCHotfixResponse response = (RPCHotfixResponse)await session.Call(new RPCHotfixRequest() { Info = "Hello Server" });
              
                Log.Info(response.Info);
                session.Dispose();
            }
        }
    }
}

