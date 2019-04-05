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
    unsafe class Trinity_EntityExtension_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            Type[] args;
            Type type = typeof(Trinity.EntityExtension);
            args = new Type[]{typeof(UnityGameFramework.Runtime.EntityComponent)};
            method = type.GetMethod("GenerateSerialId", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, GenerateSerialId_0);
            args = new Type[]{typeof(UnityGameFramework.Runtime.EntityComponent), typeof(System.Int32), typeof(Trinity.HotfixEntityData)};
            method = type.GetMethod("ShowHotfixEntity", flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, ShowHotfixEntity_1);


        }


        static StackObject* GenerateSerialId_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 1);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            UnityGameFramework.Runtime.EntityComponent @entityComponent = (UnityGameFramework.Runtime.EntityComponent)typeof(UnityGameFramework.Runtime.EntityComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = Trinity.EntityExtension.GenerateSerialId(@entityComponent);

            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static StackObject* ShowHotfixEntity_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 3);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            Trinity.HotfixEntityData @data = (Trinity.HotfixEntityData)typeof(Trinity.HotfixEntityData).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.Int32 @priority = ptr_of_this_method->Value;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 3);
            UnityGameFramework.Runtime.EntityComponent @entityComponent = (UnityGameFramework.Runtime.EntityComponent)typeof(UnityGameFramework.Runtime.EntityComponent).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack));
            __intp.Free(ptr_of_this_method);


            Trinity.EntityExtension.ShowHotfixEntity(@entityComponent, @priority, @data);

            return __ret;
        }



    }
}
