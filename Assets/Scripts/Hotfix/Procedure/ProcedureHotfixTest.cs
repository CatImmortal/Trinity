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

            //扩展测试
            //解析DateTime
            JsonParser.ExtensionParseFuncDict.Add(typeof(DateTime), () =>
            {
                RangeString rs = JsonParser.Lexer.GetNextTokenByType(TokenType.String);
                return DateTime.Parse(rs.ToString());
            });
            //转换DateTime
            JsonParser.ExtensionToJsonFuncDict.Add(typeof(DateTime), (value) =>
            {
                Util.Append("\"");
                Util.Append(value.ToString());
                Util.Append("\"");
            });

            //data = new TestJson1_Root();
            //data.b = true;
            //data.num = 3.14d;
            //data.str = "to json";
            //data.intList = new List<int>() { 1, 2, 3, 4 };
            //data.intDict = new Dictionary<string, int>() { { "key1", 1 }, { "key2", 2 } };

            //TestJson1_Item item = new TestJson1_Item();
            //item.b = true;
            //item.num = 9.99d;
            //item.str = "item";

            //data.item = item;
            //data.itemList = new List<TestJson1_Item>() { item, item };
            //data.itemDict = new Dictionary<string, TestJson1_Item>() { { "key3", item }, { "key4", item } };


            //json = JsonParser.ToJson(data);

            //TestJson2_Data data = new TestJson2_Data();
            //data.dt = DateTime.Now;
            //data.dtList = new List<DateTime>() { DateTime.Now, DateTime.UtcNow };
            //string str = JsonParser.ToJson(data);
            //Debug.Log(str);

           // JsonParser.Test(typeof(GameEntry));
            JsonParser.Test(typeof(TestJson1_Root));



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

