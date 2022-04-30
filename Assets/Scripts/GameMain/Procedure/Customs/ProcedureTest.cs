using GameFramework.Fsm;
using GameFramework.Procedure;
using System.Collections;
using System.Collections.Generic;
using CatJson;
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
            
            TestDataRoot root = new TestDataRoot()
            {
                Base = new TestDataBase() {BaseA = 1},
                Data1 = new TestData1(){A1 = 2,BaseA = 3},
                Data2 = new TestData2(){A2 = 4,BaseA = 5},
                Datas = new TestDataBase[]
                {
                    new TestDataBase() {BaseA = 6},
                    new TestData1(){A1 = 7,BaseA = 8},
                    new TestData2(){A2 = 9,BaseA = 10},
                },
                DataList = new List<TestDataBase>()
                {
                    new TestDataBase() {BaseA = 11},
                    new TestData1(){A1 = 12,BaseA = 13},
                    new TestData2(){A2 = 14,BaseA = 15},
                },
                DataDict = new Dictionary<string, TestDataBase>()
                {
                    {"key1",new TestDataBase(){BaseA =  16}},
                    {"key2",new TestData1(){A1 = 17,BaseA = 18}},
                    {"key3",new TestData2(){A2 = 19,BaseA = 20}},
                },
            };
            
            string json = JsonParser.ToJson(root);
            Debug.Log(json);
            
            TestDataRoot root2 = JsonParser.ParseJson<TestDataRoot>(json);
            Debug.Log(JsonParser.ToJson(root2));
            
            Debug.Log(JsonParser.ToJson(root) == JsonParser.ToJson(root2));
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

