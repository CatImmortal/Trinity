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
    unsafe class UnityGameFramework_Runtime_LoadSceneSuccessEventArgs_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(UnityGameFramework.Runtime.LoadSceneSuccessEventArgs);
            args = new Type[]{};
            method = type.GetMethod("get_UserData", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_UserData_0);
            args = new Type[]{};
            method = type.GetMethod("get_SceneAssetName", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, get_SceneAssetName_1);

            field = type.GetField("EventId", flag);
            app.RegisterCLRFieldGetter(field, get_EventId_0);


        }


        static StackObject* get_UserData_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityGameFramework.Runtime.LoadSceneSuccessEventArgs instance_of_this_method = (UnityGameFramework.Runtime.LoadSceneSuccessEventArgs)typeof(UnityGameFramework.Runtime.LoadSceneSuccessEventArgs).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.UserData;

            object obj_result_of_this_method = result_of_this_method;
            if(obj_result_of_this_method is CrossBindingAdaptorType)
            {    
                return ILIntepreter.PushObject(__ret, __mStack, ((CrossBindingAdaptorType)obj_result_of_this_method).ILInstance, true);
            }
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method, true);
        }

        static StackObject* get_SceneAssetName_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityGameFramework.Runtime.LoadSceneSuccessEventArgs instance_of_this_method = (UnityGameFramework.Runtime.LoadSceneSuccessEventArgs)typeof(UnityGameFramework.Runtime.LoadSceneSuccessEventArgs).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            var result_of_this_method = instance_of_this_method.SceneAssetName;

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_EventId_0(ref object o)
        {
            return UnityGameFramework.Runtime.LoadSceneSuccessEventArgs.EventId;
        }


    }
}
