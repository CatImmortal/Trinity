using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Trinity
{
    /// <summary>
    /// 绑定浮点数
    /// </summary>
    public class BindFloat : BindValue<float>
    {
        private HashSet<Slider> m_BindSliders;

        public BindFloat()
        {

        }

        public BindFloat(float value) : base(value)
        {

        }

        public override void Bind(object obj)
        {
            base.Bind(obj);

            ChangeHashSet(obj, m_BindSliders, true);
        }

        public override void Unbind(object obj)
        {
            base.Unbind(obj);

            ChangeHashSet(obj, m_BindSliders, false);

        }

        protected override void OnValueChanged()
        {
            base.OnValueChanged();

            if (m_BindSliders != null)
            {
                foreach (Slider slider in m_BindSliders)
                {
                    slider.value = Value;
                }
            }
        }



    }
}

