using UnityEngine;

//自动生成于：2019/6/12 1:02:16
namespace Trinity
{

	public class TestEntity2Data : EntityData
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
