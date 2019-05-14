using UnityEngine;

namespace Trinity.Hotfix
{
	public interface IUIFactory
	{
		UI Create(Scene scene, string type, GameObject parent);
		void Remove(string type);
	}
}