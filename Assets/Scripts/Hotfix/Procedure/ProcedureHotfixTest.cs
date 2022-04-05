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
using UnityEngine.Profiling;
namespace Trinity.Hotfix
{
    public class ProcedureHotfixTest : ProcedureBase
    {
        private TestJson1_Root data;
        private string json;
        protected internal override void OnEnter(IFsm procedureOwner)
        {
            base.OnEnter(procedureOwner);

            Debug.Log("进入了热更新测试流程");

            // TestDataRoot root = new TestDataRoot()
            // {
            //     Base = new TestDataBase() {BaseA = 1},
            //     Data1 = new TestData1(){A1 = 2,BaseA = 3},
            //     Data2 = new TestData2(){A2 = 4,BaseA = 5},
            //     Datas = new TestDataBase[]
            //     {
            //         new TestDataBase() {BaseA = 6},
            //         new TestData1(){A1 = 7,BaseA = 8},
            //         new TestData2(){A2 = 9,BaseA = 10},
            //     },
            //     DataList = new List<TestDataBase>()
            //     {
            //         new TestDataBase() {BaseA = 11},
            //         new TestData1(){A1 = 12,BaseA = 13},
            //         new TestData2(){A2 = 14,BaseA = 15},
            //     },
            //     DataDict = new Dictionary<string, TestDataBase>()
            //     {
            //         {"key1",new TestDataBase(){BaseA =  16}},
            //         {"key2",new TestData1(){A1 = 17,BaseA = 18}},
            //         {"key3",new TestData2(){A2 = 19,BaseA = 20}},
            //     },
            // };
            //
            // json = JsonParser.ToJson(root);
            // Debug.Log(json);
            //
            //
            // data = new TestJson1_Root();
            // data.b = true;
            // data.num = 3.14d;
            // data.str = "to json";
            // data.intList = new List<int>() { 1, 2, 3, 4 };
            // data.intDict = new Dictionary<string, int>() { { "key1", 1 }, { "key2", 2 } };
            //
            // TestJson1_Item item = new TestJson1_Item();
            // item.b = false;
            // item.num = 9.99d;
            // item.str = "item";
            //
            // data.item = item;
            // data.itemList = new List<TestJson1_Item>() { item, item };
            // data.itemDict = new Dictionary<string, TestJson1_Item>() { { "key3", item }, { "key4", item } };
            //
            //
            // json = JsonParser.ToJson(data);
            // Debug.Log(json);
            //
            // TestJson1_Root data2 = JsonParser.ParseJson<TestJson1_Root>(json);
            // Debug.LogError(JsonParser.ToJson(data2));
            

            
            // TestDataRoot root2 = JsonParser.ParseJson<TestDataRoot>(json);
            // Debug.Log(JsonParser.ToJson(root2));
            //
            // Debug.Log(JsonParser.ToJson(root) == JsonParser.ToJson(root2));
           
            
            // List<int> intList = new List<int>()
            // {
            //     1, 2, 3, 4,
            // };
            //
            // List<TestJson1_Item> itemList = new List<TestJson1_Item>()
            // {
            //     new TestJson1_Item() {b = true, num = 9.99d, str = "item"},
            //     new TestJson1_Item() {b = true, num = 9.99d, str = "item"},
            //     new TestJson1_Item() {b = true, num = 9.99d, str = "item"},
            // };
            
            // json = JsonParser.ToJson(intList);
            // Debug.Log(json);
            //
            // json = JsonParser.ToJson(itemList);
            // Debug.Log(json);
            
            // json = JsonParser.ToJson<TestDataBase>(new TestData1(){BaseA =  5, A1 = 6});
            // Debug.Log(json);
            
            // json = JsonParser.ToJson(new TestJson1_Item() {b = true, num = 9.99d, str = "item",BaseA = new TestData1(){BaseA = 7,A1 = 8}});
            // Debug.Log(json);
        }

        protected internal override void OnUpdate(IFsm procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (Input.GetKeyDown( KeyCode.A))
            {
                Profiler.BeginSample("CatJson");
                for (int i = 0; i < 1000; i++)
                {
                    TestJson1_Root result = JsonParser.ParseJson<TestJson1_Root>(json);
                }
                Profiler.EndSample();


                Profiler.BeginSample("LitJson");
                for (int i = 0; i < 1000; i++)
                {
                    TestJson1_Root result = JsonMapper.ToObject<TestJson1_Root>(json);
                }
                Profiler.EndSample();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Profiler.BeginSample("CatJson");
                for (int i = 0; i < 1000; i++)
                {
                    string json = JsonParser.ToJson(data);
                }
                Profiler.EndSample();


                Profiler.BeginSample("LitJson");
                for (int i = 0; i < 1000; i++)
                {
                    string json = JsonMapper.ToJson(data);
                }
                Profiler.EndSample();
            }
        }

        

    }
}

