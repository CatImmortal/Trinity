using System;
using System.Collections;
using System.Collections.Generic;
using ETModel;
using UnityEngine;

//自动生成于：2019/8/1 15:34:29
namespace Trinity.Hotfix
{

	public partial class TestUIForm : HotfixUGuiForm
	{

		public override void OnInit(Trinity.HotfixUGuiForm uiFormLogic, object userdata)
		{
			base.OnInit(uiFormLogic, userdata);

			GetObjects();

			Log.Info("hotfix OnInit");
			m_BtnTest.onClick.AddListener(OnTestBtnClick);
			m_BtnTest2.onClick.AddListener(OnTest2BtnClick);
		}

        private void OnTest2BtnClick()
        {
            Debug.Log("New btn click");
        }

        public override void OnOpen(object userdata)
        {
			base.OnOpen(userdata);

			Log.Info("hotfix OnOpen");
        }

        private void OnTestBtnClick()
        {
            Log.Info("Test hotfix");
        }

        public override void OnClose(object userdata)
        {
            Log.Info("OnClose hotfix");
        }
    }
}
