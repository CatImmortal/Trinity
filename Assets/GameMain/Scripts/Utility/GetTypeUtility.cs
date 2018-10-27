using System;
using System.Collections;
using System.Collections.Generic;
using Trinity;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameEntry = Trinity.GameEntry;

public static class GetTypeUtility {

    public static Type GetType(string typeFullName)
    {
        Type t = Type.GetType(typeFullName);
        if (t == null)
        {
            t = GameEntry.ILRuntime.GetHotfixType(typeFullName);
        }

        if (t == null)
        {
            Log.Error("想要获取的Type对象为空：{0}", typeFullName);
        }

        return t;
    }

}
