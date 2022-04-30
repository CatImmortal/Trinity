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
    unsafe class TestDataBase_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::TestDataBase);

            field = type.GetField("BaseA", flag);
            app.RegisterCLRFieldGetter(field, get_BaseA_0);
            app.RegisterCLRFieldSetter(field, set_BaseA_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_BaseA_0, AssignFromStack_BaseA_0);

            app.RegisterCLRCreateArrayInstance(type, s => new global::TestDataBase[s]);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_BaseA_0(ref object o)
        {
            return ((global::TestDataBase)o).BaseA;
        }

        static StackObject* CopyToStack_BaseA_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TestDataBase)o).BaseA;
            __ret->ObjectType = ObjectTypes.Integer;
            __ret->Value = result_of_this_method;
            return __ret + 1;
        }

        static void set_BaseA_0(ref object o, object v)
        {
            ((global::TestDataBase)o).BaseA = (System.Int32)v;
        }

        static StackObject* AssignFromStack_BaseA_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Int32 @BaseA = ptr_of_this_method->Value;
            ((global::TestDataBase)o).BaseA = @BaseA;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new global::TestDataBase();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}