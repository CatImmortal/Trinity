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
    unsafe class CatJson_JsonParser_Binding
    {
        public static void Register(ILRuntime.Runtime.Enviorment.AppDomain app)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly;
            MethodBase method;
            FieldInfo field;
            Type[] args;
            Type type = typeof(CatJson.JsonParser);
            Dictionary<string, List<MethodInfo>> genericMethods = new Dictionary<string, List<MethodInfo>>();
            List<MethodInfo> lst = null;                    
            foreach(var m in type.GetMethods())
            {
                if(m.IsGenericMethodDefinition)
                {
                    if (!genericMethods.TryGetValue(m.Name, out lst))
                    {
                        lst = new List<MethodInfo>();
                        genericMethods[m.Name] = lst;
                    }
                    lst.Add(m);
                }
            }
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance)};
            if (genericMethods.TryGetValue("ParseJson", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance), typeof(System.String), typeof(System.Boolean)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ParseJson_0);

                        break;
                    }
                }
            }
            args = new Type[]{typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance)};
            if (genericMethods.TryGetValue("ToJson", out lst))
            {
                foreach(var m in lst)
                {
                    if(m.MatchGenericParameters(args, typeof(System.String), typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance), typeof(System.Boolean)))
                    {
                        method = m.MakeGenericMethod(args);
                        app.RegisterCLRMethodRedirection(method, ToJson_1);

                        break;
                    }
                }
            }

            field = type.GetField("ExtensionParseFuncDict", flag);
            app.RegisterCLRFieldGetter(field, get_ExtensionParseFuncDict_0);
            app.RegisterCLRFieldSetter(field, set_ExtensionParseFuncDict_0);
            app.RegisterCLRFieldBinding(field, CopyToStack_ExtensionParseFuncDict_0, AssignFromStack_ExtensionParseFuncDict_0);
            field = type.GetField("ExtensionToJsonFuncDict", flag);
            app.RegisterCLRFieldGetter(field, get_ExtensionToJsonFuncDict_1);
            app.RegisterCLRFieldSetter(field, set_ExtensionToJsonFuncDict_1);
            app.RegisterCLRFieldBinding(field, CopyToStack_ExtensionToJsonFuncDict_1, AssignFromStack_ExtensionToJsonFuncDict_1);
            field = type.GetField("Lexer", flag);
            app.RegisterCLRFieldGetter(field, get_Lexer_2);
            app.RegisterCLRFieldSetter(field, set_Lexer_2);
            app.RegisterCLRFieldBinding(field, CopyToStack_Lexer_2, AssignFromStack_Lexer_2);


        }


        static StackObject* ParseJson_0(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @reflection = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            System.String @json = (System.String)typeof(System.String).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = CatJson.JsonParser.ParseJson<ILRuntime.Runtime.Intepreter.ILTypeInstance>(@json, @reflection);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static StackObject* ToJson_1(ILIntepreter __intp, StackObject* __esp, IList<object> __mStack, CLRMethod __method, bool isNewObj)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            StackObject* ptr_of_this_method;
            StackObject* __ret = ILIntepreter.Minus(__esp, 2);

            ptr_of_this_method = ILIntepreter.Minus(__esp, 1);
            System.Boolean @reflection = ptr_of_this_method->Value == 1;

            ptr_of_this_method = ILIntepreter.Minus(__esp, 2);
            ILRuntime.Runtime.Intepreter.ILTypeInstance @obj = (ILRuntime.Runtime.Intepreter.ILTypeInstance)typeof(ILRuntime.Runtime.Intepreter.ILTypeInstance).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            __intp.Free(ptr_of_this_method);


            var result_of_this_method = CatJson.JsonParser.ToJson<ILRuntime.Runtime.Intepreter.ILTypeInstance>(@obj, @reflection);

            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }


        static object get_ExtensionParseFuncDict_0(ref object o)
        {
            return CatJson.JsonParser.ExtensionParseFuncDict;
        }

        static StackObject* CopyToStack_ExtensionParseFuncDict_0(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = CatJson.JsonParser.ExtensionParseFuncDict;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_ExtensionParseFuncDict_0(ref object o, object v)
        {
            CatJson.JsonParser.ExtensionParseFuncDict = (System.Collections.Generic.Dictionary<System.Type, System.Func<System.Object>>)v;
        }

        static StackObject* AssignFromStack_ExtensionParseFuncDict_0(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.Dictionary<System.Type, System.Func<System.Object>> @ExtensionParseFuncDict = (System.Collections.Generic.Dictionary<System.Type, System.Func<System.Object>>)typeof(System.Collections.Generic.Dictionary<System.Type, System.Func<System.Object>>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            CatJson.JsonParser.ExtensionParseFuncDict = @ExtensionParseFuncDict;
            return ptr_of_this_method;
        }

        static object get_ExtensionToJsonFuncDict_1(ref object o)
        {
            return CatJson.JsonParser.ExtensionToJsonFuncDict;
        }

        static StackObject* CopyToStack_ExtensionToJsonFuncDict_1(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = CatJson.JsonParser.ExtensionToJsonFuncDict;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_ExtensionToJsonFuncDict_1(ref object o, object v)
        {
            CatJson.JsonParser.ExtensionToJsonFuncDict = (System.Collections.Generic.Dictionary<System.Type, System.Action<System.Object>>)v;
        }

        static StackObject* AssignFromStack_ExtensionToJsonFuncDict_1(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            System.Collections.Generic.Dictionary<System.Type, System.Action<System.Object>> @ExtensionToJsonFuncDict = (System.Collections.Generic.Dictionary<System.Type, System.Action<System.Object>>)typeof(System.Collections.Generic.Dictionary<System.Type, System.Action<System.Object>>).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            CatJson.JsonParser.ExtensionToJsonFuncDict = @ExtensionToJsonFuncDict;
            return ptr_of_this_method;
        }

        static object get_Lexer_2(ref object o)
        {
            return CatJson.JsonParser.Lexer;
        }

        static StackObject* CopyToStack_Lexer_2(ref object o, ILIntepreter __intp, StackObject* __ret, IList<object> __mStack)
        {
            var result_of_this_method = CatJson.JsonParser.Lexer;
            return ILIntepreter.PushObject(__ret, __mStack, result_of_this_method);
        }

        static void set_Lexer_2(ref object o, object v)
        {
            CatJson.JsonParser.Lexer = (CatJson.JsonLexer)v;
        }

        static StackObject* AssignFromStack_Lexer_2(ref object o, ILIntepreter __intp, StackObject* ptr_of_this_method, IList<object> __mStack)
        {
            ILRuntime.Runtime.Enviorment.AppDomain __domain = __intp.AppDomain;
            CatJson.JsonLexer @Lexer = (CatJson.JsonLexer)typeof(CatJson.JsonLexer).CheckCLRTypes(StackObject.ToObject(ptr_of_this_method, __domain, __mStack), (CLR.Utils.Extensions.TypeFlags)0);
            CatJson.JsonParser.Lexer = @Lexer;
            return ptr_of_this_method;
        }



    }
}
