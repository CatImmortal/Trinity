using UnityGameFramework.Runtime;

//自动生成于：2019/3/13 16:39:17
namespace Trinity
{
	public static class ShowEntityExtension
	{
		public static void ShowTestEntity(this EntityComponent entityComponent,TestEntityData data)
		{
			entityComponent.ShowEntity(typeof(TestEntity), 0, data);
		}

	}
}
