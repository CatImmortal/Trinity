using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CatJson;

//[GenJsonCodeRoot]
public class TestJson1_Root
{
    public bool b;
    public float num;
    public string str;
    public List<int> intList;
    public Dictionary<string, int> intDict;
    public TestJson1_Item item;
    public List<TestJson1_Item> itemList;
    public Dictionary<string, TestJson1_Item> itemDict;
}

public class TestJson1_Item
{
    public bool b;
    public float num;
    public string str;
}
