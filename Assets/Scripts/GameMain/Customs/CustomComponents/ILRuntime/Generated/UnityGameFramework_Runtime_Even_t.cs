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
    unsafe class UnityGameFramework_Runtime_EventComponent_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(UnityGameFramework.Runtime.EventComponent);
            args = new Type[]{typeof(System.Int32), typeof(System.EventHandler<GameFramework.Event.GameEventArgs>)};
            method = type.GetMethod("Subscribe", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Subscribe_0);
            args = new Type[]{typeof(System.Int32), typeof(System.EventHandler<GameFramework.Event.GameEventArgs>)};
            method = type.GetMethod("Unsubscribe", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Unsubscribe_1);


        }


        static StackObject* Subscribe_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler<GameFramework.Event.GameEventArgs> @handler = (System.EventHandler<GameFramework.Event.GameEventArgs>)typeof(System.EventHandler<GameFramework.Event.GameEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @id = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityGameFramework.Runtime.EventComponent instance_of_this_method = (UnityGameFramework.Runtime.EventComponent)typeof(UnityGameFramework.Runtime.EventComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Subscribe(@id, @handler);

            return __ret;
        }

        static StackObject* Unsubscribe_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.EventHandler<GameFramework.Event.GameEventArgs> @handler = (System.EventHandler<GameFramework.Event.GameEventArgs>)typeof(System.EventHandler<GameFramework.Event.GameEventArgs>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @id = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityGameFramework.Runtime.EventComponent instance_of_this_method = (UnityGameFramework.Runtime.EventComponent)typeof(UnityGameFramework.Runtime.EventComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            instance_of_this_method.Unsubscribe(@id, @handler);

            return __ret;
        }



    }
}
