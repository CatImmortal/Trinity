using System;
using System.Collections;
using System.Collections.Generic;
using ETHotfix;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using CatJson;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.CLR.TypeSystem;
using LitJson;
namespace Trinity.Hotfix
{
    public class ProcedureHotfixTest : ProcedureBase
    {


        protected internal override void OnEnter(IFsm procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Debug.Log("进入了热更新测试流程");
           
            TestJson1_Root data = new TestJson1_Root();
            data.b = true;
            data.num = 3.14f;
            data.str = "to json";
            data.intList = new List<int>() { 1, 2, 3, 4 };
            data.intDict = new Dictionary<string, int>() { { "key1", 1 }, { "key2", 2 } };

            TestJson1_Item item = new TestJson1_Item();
            item.b = true;
            item.num = 9.99f;
            item.str = "item";

            data.item = item;
            data.itemList = new List<TestJson1_Item>() { item, item };
            data.itemDict = new Dictionary<string, TestJson1_Item>() { { "key3", item }, { "key4", item } };

            string json = JsonParser.ToJson(data);

            Debug.Log(json);
            //TestJson1_Root data2 = JsonMapper.ToObject<TestJson1_Root>(json);
            //TestJson1_Root data3 = JsonParser.ParseJson<TestJson1_Root>(json);
            //Debug.Log(data3.b);
            //Debug.Log(data3.num);
            //Debug.Log(data3.str);
            //Debug.Log(data3.item);
            //for (int i = 0; i < data3.intList.Count; i++)
            //{
            //   Debug.Log(data3.intList[i]);
            //}
            //foreach (var dictItem in data3.intDict)
            //{
            //    Debug.Log(dictItem);
            //}
            //for (int i = 0; i < data3.itemList.Count; i++)
            //{
            //    Debug.Log(data3.itemList[i]);
            //}
            //foreach (var dictItem in data3.itemDict)
            //{
            //    Debug.Log(dictItem);
            //}
        }

        protected internal override async void OnUpdate(IFsm procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            //if (Input.GetKeyDown(KeyCode.A))
            //{
            //    Session session = HotfixGameEntry.ETNetwork.CreateHotfixSession();
            //    session.Send(new HotfixTestMessage() { Info = "6666" });

            //    HotfixRpcResponse response = (HotfixRpcResponse)await session.Call(new HotfixRpcRequest() { Info = "Hello Server" });
            //    Debug.Log(response.Info);
            //    session.Dispose();
            //}
        }

        

    }
}

