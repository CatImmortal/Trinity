using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Trinity
{
    /// <summary>
    /// UI绑定扩展
    /// </summary>
    public static class UIBindExtension
    {
        public static void BindValue<T>(this Text self,BindValue<T> value)
        {
            value.Bind(self);
        }

        public static void UnbindValue<T>(this Text self, BindValue<T> value)
        {
            value.Unbind(self);
        }

        public static void BindValue<T>(this Slider self, BindValue<T> value)
        {
            value.Bind(self);
        }

        public static void UnbindValue<T>(this Slider self, BindValue<T> value)
        {
            value.Unbind(self);
        }

        public static void BindValue<T>(this InputField self, BindValue<T> value)
        {
            value.Bind(self);
        }

        public static void UnbindValue<T>(this InputField self, BindValue<T> value)
        {
            value.Unbind(self);
        }
    }
}


