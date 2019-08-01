using UnityEngine;
using UnityEngine.UI;

//自动生成于：2019/8/1 16:44:54
namespace Trinity.Hotfix
{

	public partial class TestUIForm : HotfixUGuiForm
	{

		private Button m_BtnTest;
		private Button m_BtnTest2;

		private void GetObjects()
		{
			ReferenceCollector rc = UIFormLogic.GetComponent<ReferenceCollector>();

			m_BtnTest = rc.GetObj<Button>(0);
			m_BtnTest2 = rc.GetObj<Button>(1);
		}

	}
}
