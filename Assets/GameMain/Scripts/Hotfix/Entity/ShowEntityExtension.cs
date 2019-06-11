using System.Threading.Tasks;
using UnityGameFramework.Runtime;

//自动生成于：2019/6/12 1:02:46
namespace Trinity.Hotfix
{
	public static class ShowEntityExtension
	{
		public static void ShowTestEntity(this EntityComponent entityComponent,TestEntityData data)
		{
			Trinity.HotfixEntityData tData = GameFramework.ReferencePool.Acquire<Trinity.HotfixEntityData>();
			tData.Fill(data.Id,data.TypeId,"TestEntity",data);
			tData.Position = data.Position;
			tData.Rotation = data.Rotation;

			entityComponent.ShowHotfixEntity(0, tData);
		}

		public static async Task<Entity> AwaitShowTestEntity(this EntityComponent entityComponent,TestEntityData data)
		{
			Trinity.HotfixEntityData tData = GameFramework.ReferencePool.Acquire<Trinity.HotfixEntityData>();
			tData.Fill(data.Id,data.TypeId,"TestEntity",data);
			tData.Position = data.Position;
			tData.Rotation = data.Rotation;

			Entity entity = await entityComponent.AwaitShowHotfixEntity(0, tData);
			return entity;
		}

		public static void ShowTestEntity2(this EntityComponent entityComponent,TestEntity2Data data)
		{
			Trinity.HotfixEntityData tData = GameFramework.ReferencePool.Acquire<Trinity.HotfixEntityData>();
			tData.Fill(data.Id,data.TypeId,"TestEntity2",data);
			tData.Position = data.Position;
			tData.Rotation = data.Rotation;

			entityComponent.ShowHotfixEntity(0, tData);
		}

		public static async Task<Entity> AwaitShowTestEntity2(this EntityComponent entityComponent,TestEntity2Data data)
		{
			Trinity.HotfixEntityData tData = GameFramework.ReferencePool.Acquire<Trinity.HotfixEntityData>();
			tData.Fill(data.Id,data.TypeId,"TestEntity2",data);
			tData.Position = data.Position;
			tData.Rotation = data.Rotation;

			Entity entity = await entityComponent.AwaitShowHotfixEntity(0, tData);
			return entity;
		}

	}
}
