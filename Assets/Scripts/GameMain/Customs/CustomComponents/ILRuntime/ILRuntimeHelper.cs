using ETModel;
using Google.Protobuf;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Generated;
using ILRuntime.Runtime.Intepreter;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using AppDomain = ILRuntime.Runtime.Enviorment.AppDomain;

namespace Trinity
{
    public static class ILRuntimeHelper
    {
        public static void InitILRuntime(AppDomain appDomain)
        {
            //TODO:注册重定向方法

            //TODO:适配委托

            //GF用
            appDomain.DelegateManager.RegisterMethodDelegate<float>();
            appDomain.DelegateManager.RegisterMethodDelegate<object, ILTypeInstance>();
            appDomain.DelegateManager.RegisterMethodDelegate<object, GameFramework.Event.GameEventArgs>();


            //ET用
            appDomain.DelegateManager.RegisterMethodDelegate<List<object>>();
            appDomain.DelegateManager.RegisterMethodDelegate<AChannel, System.Net.Sockets.SocketError>();
            appDomain.DelegateManager.RegisterMethodDelegate<byte[], int, int>();
            appDomain.DelegateManager.RegisterMethodDelegate<IResponse>();
            appDomain.DelegateManager.RegisterMethodDelegate<Session, object>();
            appDomain.DelegateManager.RegisterMethodDelegate<Session, byte, ushort, MemoryStream>();
            appDomain.DelegateManager.RegisterMethodDelegate<Session>();
            appDomain.DelegateManager.RegisterMethodDelegate<ILTypeInstance>();
            appDomain.DelegateManager.RegisterMethodDelegate<Session, ushort, MemoryStream>();

            //PB用
            appDomain.DelegateManager.RegisterFunctionDelegate<IMessageAdaptor.Adaptor>();
            appDomain.DelegateManager.RegisterMethodDelegate<IMessageAdaptor.Adaptor>();


            //TODO:注册委托
            appDomain.DelegateManager.RegisterDelegateConvertor<UnityAction>((action) =>
            {
                return new UnityAction(() =>
                {
                    ((Action)action)();
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<UnityAction<float>>((action) =>
            {
                return new UnityAction<float>((a) =>
                {
                    ((Action<float>)action)(a);
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<EventHandler<GameFramework.Event.GameEventArgs>>((act) =>
            {
                return new EventHandler<GameFramework.Event.GameEventArgs>((sender, e) =>
                {
                    ((Action<object, GameFramework.Event.GameEventArgs>)act)(sender, e);
                });
            });

            appDomain.DelegateManager.RegisterDelegateConvertor<EventHandler<ILTypeInstance>>((act) =>
            {
                return new EventHandler<ILTypeInstance>((sender, e) =>
                {
                    ((Action<object, ILTypeInstance>)act)(sender, e);
                });
            });


            //注册CLR绑定代码
            CLRBindings.Initialize(appDomain);

            //TODO:注册跨域继承适配器
            appDomain.RegisterCrossBindingAdaptor(new IMessageAdaptor());
            appDomain.RegisterCrossBindingAdaptor(new IDisposableAdaptor());
            appDomain.RegisterCrossBindingAdaptor(new IAsyncStateMachineAdaptor());

            //TODO:注册值类型绑定
            appDomain.RegisterValueTypeBinder(typeof(Vector3), new Vector3Binder());
            appDomain.RegisterValueTypeBinder(typeof(Quaternion), new QuaternionBinder());
            appDomain.RegisterValueTypeBinder(typeof(Vector2), new Vector2Binder());

            //注册LitJson
            LitJson.JsonMapper.RegisterILRuntimeCLRRedirection(appDomain);
        }
    }
}

