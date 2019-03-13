using UnityEngine;
using GameFramework;

//自动生成于：2019/3/13 16:39:15
namespace Trinity
{

	public class TestEntity : Entity
	{

		private TestEntityData m_TestEntityData;

		protected override void OnShow(object userData)
		{
			base.OnShow(userData);
			m_TestEntityData = (TestEntityData)userData;
		}

		protected override void OnHide(object userData)
		{
			base.OnHide(userData);
			ReferencePool.Release(m_TestEntityData);
		}
	}
}
