using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDataRoot
{
    public TestDataBase Base;
    public TestDataBase Data1;
    public TestDataBase Data2;
    public TestDataBase[] Datas;
    public List<TestDataBase> DataList;
    public Dictionary<string, TestDataBase> DataDict;
}

public class TestDataBase
{
    public int BaseA;
}

public class TestData1 : TestDataBase
{
    public int A1;
}


public class TestData2 : TestDataBase
{
    public int A2;
}

