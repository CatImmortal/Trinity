using System.Collections;
using System.Collections.Generic;
using ETModel;
using UnityEngine;

//自动生成于：2019/8/1 15:50:41
namespace Trinity
{

	public partial class TestUIForm : HotfixUGuiForm
	{

		protected override void OnInit(object userdata)
		{
			base.OnInit(userdata);

			GetObjects();

			m_BtnTest.onClick.AddListener(OnTestBtnClick);

			Log.Info("base OnInit");
		}

		protected override void OnOpen(object userData)
		{
			base.OnOpen(userData);

			Log.Info("base OnOpen");
		}

        private void OnTestBtnClick()
        {
            Log.Info("Test base");
        }
	}
}
