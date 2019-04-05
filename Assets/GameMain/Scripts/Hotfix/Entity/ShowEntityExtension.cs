using UnityGameFramework.Runtime;

//自动生成于：2019/3/13 16:40:03
namespace Trinity.Hotfix
{
	public static class ShowEntityExtension
	{
		public static void ShowTestEntity2(this EntityComponent entityComponent,TestEntity2Data data)
		{
			Trinity.HotfixEntityData tData = GameFramework.ReferencePool.Acquire<Trinity.HotfixEntityData>();
			tData.Fill(data.Id,data.TypeId,"TestEntity2",data);
			tData.Position = data.Position;
			tData.Rotation = data.Rotation;

			entityComponent.ShowHotfixEntity(0, tData);
		}

	}
}
