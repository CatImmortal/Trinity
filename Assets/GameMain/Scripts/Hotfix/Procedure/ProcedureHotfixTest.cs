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
        Session hotfixSession;

        protected internal override void OnEnter(IFsm procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Log.Info("进入了热更新测试流程");

            GameEntry.Entity.ShowTestEntity2(ReferencePool.Acquire<TestEntity2Data>().Fill(2));

            GameEntry.Event.Subscribe(HotfixTestEventArgs.EventId, OnHotfixTest);
        }

        private void OnHotfixTest(object sender, GameEventArgs e)
        {
            HotfixTestEventArgs ne = (HotfixTestEventArgs)e;
            Log.Info(ne.Str);
        }

        protected internal override void OnUpdate(IFsm procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (Input.GetMouseButtonDown(0))
            {
                GameEntry.Event.Fire(this, ReferencePool.Acquire<HotfixTestEventArgs>().Fill("热更新事件测试"));
            }
        }

        private async void RPCTest()
        {
            ETModel.Session session = GameEntry.ETNetwork.CreateSession(GameEntry.ETNetwork.ServerIP);
            ETModel.R2C_RPCTest rpcTestResponse = (ETModel.R2C_RPCTest)await session.Call(new ETModel.C2R_RPCTest() { Text = "Hello ETServer" });
            Log.Info("收到了服务端的RPC消息响应："  + rpcTestResponse.Text);
        }

    }
}

