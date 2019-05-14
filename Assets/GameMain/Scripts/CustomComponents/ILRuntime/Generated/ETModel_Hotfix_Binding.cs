using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

using ILRuntime.CLR.TypeSystem;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using ILRuntime.Runtime.Stack;
using ILRuntime.Reflection;
using ILRuntime.CLR.Utils;

namespace ILRuntime.Runtime.Generated
{
    unsafe class ETModel_Hotfix_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            FieldInfo field;
            Type[] args;
            Type type = typeof(ETModel.Hotfix);

            field = type.GetField("Update", flag);
            app.RegisterCLRFieldGetter(field, get_Update_0);
            app.RegisterCLRFieldSetter(field, set_Update_0);
            field = type.GetField("LateUpdate", flag);
            app.RegisterCLRFieldGetter(field, get_LateUpdate_1);
            app.RegisterCLRFieldSetter(field, set_LateUpdate_1);
            field = type.GetField("OnApplicationQuit", flag);
            app.RegisterCLRFieldGetter(field, get_OnApplicationQuit_2);
            app.RegisterCLRFieldSetter(field, set_OnApplicationQuit_2);


        }



        static object get_Update_0(ref object o)
        {
            return ((ETModel.Hotfix)o).Update;
        }
        static void set_Update_0(ref object o, object v)
        {
            ((ETModel.Hotfix)o).Update = (System.Action)v;
        }
        static object get_LateUpdate_1(ref object o)
        {
            return ((ETModel.Hotfix)o).LateUpdate;
        }
        static void set_LateUpdate_1(ref object o, object v)
        {
            ((ETModel.Hotfix)o).LateUpdate = (System.Action)v;
        }
        static object get_OnApplicationQuit_2(ref object o)
        {
            return ((ETModel.Hotfix)o).OnApplicationQuit;
        }
        static void set_OnApplicationQuit_2(ref object o, object v)
        {
            ((ETModel.Hotfix)o).OnApplicationQuit = (System.Action)v;
        }


    }
}
