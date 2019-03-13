using UnityEngine;

//自动生成于：2019/3/13 16:40:02
namespace Trinity.Hotfix
{

	public class TestEntity2Data : HotfixEntityData
	{

		public TestEntity2Data()
		{
		}

		public TestEntity2Data Fill(int typeId)
		{
			Fill(GameEntry.Entity.GenerateSerialId(),typeId);
			return this;
		}

		public override void Clear()
		{
			base.Clear();
		}

	}
}
