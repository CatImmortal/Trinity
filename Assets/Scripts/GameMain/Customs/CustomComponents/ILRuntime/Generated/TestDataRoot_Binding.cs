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
    unsafe class TestDataRoot_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(global::TestDataRoot);

            field = type.GetField("Base", flag);
            app.RegisterCLRFieldGetter(field, get_Base_0);
            app.RegisterCLRFieldSetter(field, set_Base_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_Base_0, AssignFromStack_Base_0);
            field = type.GetField("Data1", flag);
            app.RegisterCLRFieldGetter(field, get_Data1_1);
            app.RegisterCLRFieldSetter(field, set_Data1_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_Data1_1, AssignFromStack_Data1_1);
            field = type.GetField("Data2", flag);
            app.RegisterCLRFieldGetter(field, get_Data2_2);
            app.RegisterCLRFieldSetter(field, set_Data2_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_Data2_2, AssignFromStack_Data2_2);
            field = type.GetField("Datas", flag);
            app.RegisterCLRFieldGetter(field, get_Datas_3);
            app.RegisterCLRFieldSetter(field, set_Datas_3);
            app.RegisterCLRFieldBinding(field, CopyToStack_Datas_3, AssignFromStack_Datas_3);
            field = type.GetField("DataList", flag);
            app.RegisterCLRFieldGetter(field, get_DataList_4);
            app.RegisterCLRFieldSetter(field, set_DataList_4);
            app.RegisterCLRFieldBinding(field, CopyToStack_DataList_4, AssignFromStack_DataList_4);
            field = type.GetField("DataDict", flag);
            app.RegisterCLRFieldGetter(field, get_DataDict_5);
            app.RegisterCLRFieldSetter(field, set_DataDict_5);
            app.RegisterCLRFieldBinding(field, CopyToStack_DataDict_5, AssignFromStack_DataDict_5);

            args = new Type[]{};
            method = type.GetConstructor(flag, null, args, null);
            app.RegisterCLRMethodRedirection(method, Ctor_0);

        }



        static object get_Base_0(ref object o)
        {
            return ((global::TestDataRoot)o).Base;
        }

        static StackObject* CopyToStack_Base_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TestDataRoot)o).Base;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Base_0(ref object o, object v)
        {
            ((global::TestDataRoot)o).Base = (global::TestDataBase)v;
        }

        static StackObject* AssignFromStack_Base_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::TestDataBase @Base = (global::TestDataBase)typeof(global::TestDataBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::TestDataRoot)o).Base = @Base;
            return ptr_of_this_method;
        }

        static object get_Data1_1(ref object o)
        {
            return ((global::TestDataRoot)o).Data1;
        }

        static StackObject* CopyToStack_Data1_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TestDataRoot)o).Data1;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Data1_1(ref object o, object v)
        {
            ((global::TestDataRoot)o).Data1 = (global::TestDataBase)v;
        }

        static StackObject* AssignFromStack_Data1_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::TestDataBase @Data1 = (global::TestDataBase)typeof(global::TestDataBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::TestDataRoot)o).Data1 = @Data1;
            return ptr_of_this_method;
        }

        static object get_Data2_2(ref object o)
        {
            return ((global::TestDataRoot)o).Data2;
        }

        static StackObject* CopyToStack_Data2_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TestDataRoot)o).Data2;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Data2_2(ref object o, object v)
        {
            ((global::TestDataRoot)o).Data2 = (global::TestDataBase)v;
        }

        static StackObject* AssignFromStack_Data2_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::TestDataBase @Data2 = (global::TestDataBase)typeof(global::TestDataBase).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::TestDataRoot)o).Data2 = @Data2;
            return ptr_of_this_method;
        }

        static object get_Datas_3(ref object o)
        {
            return ((global::TestDataRoot)o).Datas;
        }

        static StackObject* CopyToStack_Datas_3(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TestDataRoot)o).Datas;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Datas_3(ref object o, object v)
        {
            ((global::TestDataRoot)o).Datas = (global::TestDataBase[])v;
        }

        static StackObject* AssignFromStack_Datas_3(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            global::TestDataBase[] @Datas = (global::TestDataBase[])typeof(global::TestDataBase[]).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::TestDataRoot)o).Datas = @Datas;
            return ptr_of_this_method;
        }

        static object get_DataList_4(ref object o)
        {
            return ((global::TestDataRoot)o).DataList;
        }

        static StackObject* CopyToStack_DataList_4(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TestDataRoot)o).DataList;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_DataList_4(ref object o, object v)
        {
            ((global::TestDataRoot)o).DataList = (System.Collections.Generic.List<global::TestDataBase>)v;
        }

        static StackObject* AssignFromStack_DataList_4(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.List<global::TestDataBase> @DataList = (System.Collections.Generic.List<global::TestDataBase>)typeof(System.Collections.Generic.List<global::TestDataBase>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::TestDataRoot)o).DataList = @DataList;
            return ptr_of_this_method;
        }

        static object get_DataDict_5(ref object o)
        {
            return ((global::TestDataRoot)o).DataDict;
        }

        static StackObject* CopyToStack_DataDict_5(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = ((global::TestDataRoot)o).DataDict;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_DataDict_5(ref object o, object v)
        {
            ((global::TestDataRoot)o).DataDict = (System.Collections.Generic.Dictionary<System.String, global::TestDataBase>)v;
        }

        static StackObject* AssignFromStack_DataDict_5(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.Dictionary<System.String, global::TestDataBase> @DataDict = (System.Collections.Generic.Dictionary<System.String, global::TestDataBase>)typeof(System.Collections.Generic.Dictionary<System.String, global::TestDataBase>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            ((global::TestDataRoot)o).DataDict = @DataDict;
            return ptr_of_this_method;
        }


        static StackObject* Ctor_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* __ret = ILIntepreter.Minus(__esp, 0);

            var result_of_this_method = new global::TestDataRoot();

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


    }
}
