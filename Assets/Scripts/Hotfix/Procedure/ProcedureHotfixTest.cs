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
            // TestDataRoot root2 = JsonParser.ParseJson<TestDataRoot>(json);
            // Debug.Log(JsonParser.ToJson(root2));
            //
            // Debug.Log(JsonParser.ToJson(root) == JsonParser.ToJson(root2));


            // HotfixTestDataRoot hotfixRoot = new HotfixTestDataRoot()
            // {
            //     Base = new TestDataBase() {BaseA = 1},
            //     Data1 = new TestData1(){A1 = 2,BaseA = 3},
            //     HotDataBase = new HotfixTestDataBase(){BaseA = 4},
            //     HotData1 = new HotfixTestData1(){A1 = 4,BaseA = 5},
            //     
            //     Datas = new HotfixTestDataBase[]
            //     {
            //         new HotfixTestDataBase() {BaseA = 6},
            //         new HotfixTestData1(){A1 = 7,BaseA = 8},
            //         new HotfixTestData2(){A2 = 9,BaseA = 10},
            //     },
            //     DataList = new List<HotfixTestDataBase>()
            //     {
            //         new HotfixTestDataBase() {BaseA = 11},
            //         new HotfixTestData1(){A1 = 12,BaseA = 13},
            //         new HotfixTestData2(){A2 = 14,BaseA = 15},
            //     },
            //     DataDict = new Dictionary<string, HotfixTestDataBase>()
            //     {
            //         {"key1",new HotfixTestDataBase(){BaseA =  16}},
            //         {"key2",new HotfixTestData1(){A1 = 17,BaseA = 18}},
            //         {"key3",new HotfixTestData2(){A2 = 19,BaseA = 20}},
            //     },
            // };
            //
            // json = JsonParser.ToJson(hotfixRoot);
            // Debug.Log(json);
            //
            // HotfixTestDataRoot hotfixRoot2 = JsonParser.ParseJson<HotfixTestDataRoot>(json);
            // Debug.Log(JsonParser.ToJson(hotfixRoot2));
            //
            // Debug.Log(JsonParser.ToJson(hotfixRoot) == JsonParser.ToJson(hotfixRoot2));
            //
            Debug.Log(JsonParser.ToJson<TestDataBase>(new TestData1(){BaseA = 1,A1 = 1}));
            Debug.Log(JsonParser.ToJson<HotfixTestDataBase>(new HotfixTestData1(){BaseA = 1,A1 = 1}));
            
            List<TestDataBase> list1 = new List<TestDataBase>()
            {
                new TestData1(){BaseA = 1,A1 = 1},
            };
            Debug.Log(JsonParser.ToJson(list1));
        }

        protected internal override void OnUpdate(IFsm procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            // if (Input.GetKeyDown( KeyCode.A))
            // {
            //     Profiler.BeginSample("CatJson");
            //     for (int i = 0; i < 1000; i++)
            //     {
            //         TestJson1_Root result = JsonParser.ParseJson<TestJson1_Root>(json);
            //     }
            //     Profiler.EndSample();
            //
            //
            //     Profiler.BeginSample("LitJson");
            //     for (int i = 0; i < 1000; i++)
            //     {
            //         TestJson1_Root result = JsonMapper.ToObject<TestJson1_Root>(json);
            //     }
            //     Profiler.EndSample();
            // }
            //
            // if (Input.GetKeyDown(KeyCode.S))
            // {
            //     Profiler.BeginSample("CatJson");
            //     for (int i = 0; i < 1000; i++)
            //     {
            //         string json = JsonParser.ToJson(data);
            //     }
            //     Profiler.EndSample();
            //
            //
            //     Profiler.BeginSample("LitJson");
            //     for (int i = 0; i < 1000; i++)
            //     {
            //         string json = JsonMapper.ToJson(data);
            //     }
            //     Profiler.EndSample();
            // }
        }

        

    }
}

