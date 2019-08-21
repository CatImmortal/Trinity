namespace ETHotfix
{
	public interface IDisposable
	{
		void Dispose();
	}


	public abstract class Object
    { 

		public override string ToString()
		{
			return GameFramework.Utility.Json.ToJson(this);
		}
	}
}