using UnityEngine;
using UnityEngine.UI;

//自动生成于：2019/8/1 15:50:41
namespace Trinity
{

	public partial class TestUIForm : HotfixUGuiForm
	{

		private Button m_BtnTest;

		private void GetObjects()
		{
			ReferenceCollector rc = GetComponent<ReferenceCollector>();

			m_BtnTest = rc.GetObj<Button>(0);
		}

	}
}
