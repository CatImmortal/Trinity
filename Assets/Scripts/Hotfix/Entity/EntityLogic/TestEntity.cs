using UnityEngine;

//自动生成于：2019/6/12 1:02:38
namespace Trinity.Hotfix
{

	public class TestEntity : HotfixEntityLogic
	{

		private TestEntityData m_TestEntityData;

		public override void OnShow(object userData)
		{
			base.OnShow(userData);
			m_TestEntityData = (TestEntityData)userData;
		}

		public override void OnHide(object userData)
		{
			base.OnHide(userData);
			ReferencePool.Release(m_TestEntityData);
		}
	}
}
