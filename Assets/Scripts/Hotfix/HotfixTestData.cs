using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HotfixTestDataRoot
{
    public TestDataBase Base;
    public TestDataBase Data1;

    public HotfixTestDataBase HotDataBase;
    public HotfixTestDataBase HotData1;
    
    public HotfixTestDataBase[] Datas;
    public List<HotfixTestDataBase> DataList;
    public Dictionary<string, HotfixTestDataBase> DataDict;
}

public class HotfixTestDataBase
{
    public int BaseA;
}

public class HotfixTestData1 : HotfixTestDataBase
{
    public int A1;
}


public class HotfixTestData2 : HotfixTestDataBase
{
    public int A2;
}

