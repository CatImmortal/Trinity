using UnityEngine;

//自动生成于：2019/3/13 16:39:15
namespace Trinity
{

	public class TestEntityData : EntityData
	{

		public TestEntityData()
		{
		}

		public TestEntityData Fill(int typeId)
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
