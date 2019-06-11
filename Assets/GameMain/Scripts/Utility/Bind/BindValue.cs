using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Trinity
{
    /// <summary>
    /// 绑定数据
    /// </summary>
    public abstract class BindValue<T>
    {
        private HashSet<Action<T>> m_Handlers;

        private HashSet<Text> m_BindTexts;

        private HashSet<InputField> m_BindInputs;

        private T m_Value;

        public T Value
        {
            get
            {
                return m_Value;
            }

            set
            {
                if (!m_Value.Equals(value))
                {
                    m_Value = value;
                    OnValueChanged();
                }
            }
        }

        protected BindValue()
        {

        }

        protected BindValue(T value)
        {
            m_Value = value;
        }

        public virtual void Bind(object obj)
        {
            ChangeHashSet(obj, m_Handlers, true);
            ChangeHashSet(obj, m_BindTexts, true);
            ChangeHashSet(obj, m_BindInputs, true);
        }

        public virtual void Unbind(object obj)
        {
            ChangeHashSet(obj, m_Handlers, false);
            ChangeHashSet(obj, m_BindTexts, false);
            ChangeHashSet(obj, m_BindInputs, false);
        }

        protected void ChangeHashSet<TElement>(object obj,HashSet<TElement> set,bool isAdd)
        {
            if (obj is TElement)
            {
                TElement node = (TElement)obj;

                if (set == null)
                {
                    set = new HashSet<TElement>();
                }

                if (isAdd)
                {
                    set.Add(node);
                    OnValueChanged();
                }
                else
                {
                    set.Remove(node);
                }
            }
        }

        protected virtual void OnValueChanged()
        {
            if (m_Handlers != null)
            {
                foreach (Action<T> handler in m_Handlers)
                {
                    handler(m_Value);
                }
            }

            if (m_BindTexts != null)
            {
                foreach (Text text in m_BindTexts)
                {
                    text.text = Value.ToString();
                }
            }

            if (m_BindInputs != null)
            {
                foreach (InputField input in m_BindInputs)
                {
                    input.text = Value.ToString();
                }
            }
        }

    }
}

