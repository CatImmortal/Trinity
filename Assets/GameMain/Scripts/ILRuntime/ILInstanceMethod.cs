using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Trinity
{
    /// <summary>
    /// ILRuntime实例方法
    /// </summary>
    public class ILInstanceMethod
    {

        /// <summary>
        /// 热更新层实例
        /// </summary>
        private object m_HotfixInstance;

        /// <summary>
        /// 热更新层方法
        /// </summary>
        private IMethod m_Method;

        /// <summary>
        /// 方法参数缓存
        /// </summary>
        private object[] m_Params;

        public ILInstanceMethod(object hotfixInstance,string typeName, string methodName, int paramCount)
        {
            m_HotfixInstance = hotfixInstance;
            m_Method = GameEntry.ILRuntime.AppDomain.LoadedTypes[typeName].GetMethod(methodName, paramCount);
            m_Params = new object[paramCount];
        }

        public void Invoke()
        {
            GameEntry.ILRuntime.AppDomain.Invoke(m_Method, m_HotfixInstance, m_Params);
        }

        public void Invoke(object a)
        {
            m_Params[0] = a;
            GameEntry.ILRuntime.AppDomain.Invoke(m_Method, m_HotfixInstance, m_Params);
        }

        public void Invoke(object a,object b)
        {
            m_Params[0] = a;
            m_Params[1] = b;
            GameEntry.ILRuntime.AppDomain.Invoke(m_Method, m_HotfixInstance, m_Params);
        }

        public void Invoke(object a,object b,object c)
        {
            m_Params[0] = a;
            m_Params[1] = b;
            m_Params[2] = c;
            GameEntry.ILRuntime.AppDomain.Invoke(m_Method, m_HotfixInstance, m_Params);
        }
    }
}

