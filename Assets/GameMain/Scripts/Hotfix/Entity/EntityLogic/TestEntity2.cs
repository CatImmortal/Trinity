using UnityEngine;

//自动生成于：2019/3/13 16:40:02
namespace Trinity.Hotfix
{

	public class TestEntity2 : HotfixEntity
	{

		private TestEntity2Data m_TestEntity2Data;

		public override void OnShow(object userData)
		{
			base.OnShow(userData);
			m_TestEntity2Data = (TestEntity2Data)userData;
		}

		public override void OnHide(object userData)
		{
			base.OnHide(userData);
			ReferencePool.Release(m_TestEntity2Data);
		}
	}
}
