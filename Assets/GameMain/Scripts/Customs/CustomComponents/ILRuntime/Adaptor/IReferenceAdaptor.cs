using GameFramework;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trinity
{
    public class IReferenceAdaptor : CrossBindingAdaptor
    {
        public override Type BaseCLRType
        {
            get
            {
                return typeof(IReference);
            }
        }

        public override Type AdaptorType
        {
            get
            {
                return typeof(Adaptor);
            }
        }

        public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
        {
            return new Adaptor(appdomain, instance);
        }

        public class Adaptor : IReference, CrossBindingAdaptorType
        {
            private ILTypeInstance m_Instance;
            private ILRuntime.Runtime.Enviorment.AppDomain m_AppDomain;

            private IMethod m_Clear;

            public Adaptor()
            {

            }

            public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
            {
                m_AppDomain = appdomain;
                m_Instance = instance;
            }

            public ILTypeInstance ILInstance
            {
                get
                {
                    return m_Instance;
                }
            }

            public void Clear()
            {
                if (m_Clear == null)
                {
                    m_Clear = m_Instance.Type.GetMethod("Clear", 0);
                }

                m_AppDomain.Invoke(m_Clear, m_Instance, null);
            }
        }
    }
}

