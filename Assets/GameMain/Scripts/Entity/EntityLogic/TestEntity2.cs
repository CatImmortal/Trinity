using UnityEngine;
using GameFramework;

//自动生成于：2019/6/12 1:02:16
namespace Trinity
{

	public class TestEntity2 : EntityLogic
	{

		private TestEntity2Data m_TestEntity2Data;

		protected override void OnShow(object userData)
		{
			base.OnShow(userData);
			m_TestEntity2Data = (TestEntity2Data)userData;
		}

		protected override void OnHide(object userData)
		{
			base.OnHide(userData);
			ReferencePool.Release(m_TestEntity2Data);
		}
	}
}
