using System.Threading.Tasks;
using UnityGameFramework.Runtime;

//自动生成于：2019/6/12 1:02:26
namespace Trinity
{
	public static class ShowEntityExtension
	{
		public static void ShowTestEntity(this EntityComponent entityComponent,TestEntityData data)
		{
			entityComponent.ShowEntity(typeof(TestEntity), 0, data);
		}

		public static async Task<Entity> AwaitShowTestEntity(this EntityComponent entityComponent,TestEntityData data)
		{
			Entity entity = await entityComponent.AwaitShowEntity(typeof(TestEntity), 0, data);
			return entity;
		}

		public static void ShowTestEntity2(this EntityComponent entityComponent,TestEntity2Data data)
		{
			entityComponent.ShowEntity(typeof(TestEntity2), 0, data);
		}

		public static async Task<Entity> AwaitShowTestEntity2(this EntityComponent entityComponent,TestEntity2Data data)
		{
			Entity entity = await entityComponent.AwaitShowEntity(typeof(TestEntity2), 0, data);
			return entity;
		}

	}
}
