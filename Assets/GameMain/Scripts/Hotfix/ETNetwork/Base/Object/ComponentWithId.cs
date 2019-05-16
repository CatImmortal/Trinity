using System;
using MongoDB.Bson.Serialization.Attributes;

namespace ETHotfix
{
	public abstract class ComponentWithId : Component
	{
		public long Id { get; set; }

		protected ComponentWithId()
		{
		}

		protected ComponentWithId(long id)
		{
			this.Id = id;
		}

		public override void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			base.Dispose();
		}
	}
}