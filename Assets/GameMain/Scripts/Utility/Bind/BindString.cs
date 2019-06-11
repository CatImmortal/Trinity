using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Trinity
{
    /// <summary>
    /// 绑定字符串
    /// </summary>
    public class BindString : BindValue<string>
    {

        public BindString()
        {

        }

        public BindString(string value) : base(value)
        {

        }

    }
}

