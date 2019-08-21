using System;
using ETModel;
using MongoDB.Bson.Serialization.Attributes;
#if !SERVER
using UnityEngine;
#endif

namespace ETHotfix
{
	public abstract class Component : Object, IDisposable
	{
		public long InstanceId { get; private set; }
		
#if !SERVER
		//public static GameObject Global { get; } = GameObject.Find("/Global");
		public GameObject GameObject { get; protected set; }
#endif

		private bool isFromPool;

		public bool IsFromPool
		{
			get
			{
				return this.isFromPool;
			}
			set
			{
				this.isFromPool = value;

				if (!this.isFromPool)
				{
					return;
				}

				if (this.InstanceId == 0)
				{
					this.InstanceId = IdGenerater.GenerateInstanceId();
				}
			}
		}

		public bool IsDisposed
		{
			get
			{
				return this.InstanceId == 0;
			}
		}

		private Component parent;
		
		public Component Parent
		{
			get
			{
				return this.parent;
			}
			set
			{
				this.parent = value;

#if !SERVER
				//if (this.parent == null)
				//{
				//	this.GameObject.transform.SetParent(Global.transform, false);
				//	return;
				//}

				//if (this.GameObject != null && this.parent.GameObject != null)
				//{
				//	this.GameObject.transform.SetParent(this.parent.GameObject.transform, false);
				//}
#endif
			}
		}

		public T GetParent<T>() where T : Component
		{
			return this.Parent as T;
		}

		public Entity Entity
		{
			get
			{
				return this.Parent as Entity;
			}
		}
		
		protected Component()
		{
			this.InstanceId = IdGenerater.GenerateInstanceId();
#if !SERVER
			//if (!this.GetType().IsDefined(typeof(HideInHierarchy), true))
			//{
			//	this.GameObject = new GameObject();
			//	this.GameObject.name = this.GetType().Name;
			//	this.GameObject.layer = LayerNames.GetLayerInt(LayerNames.HIDDEN);
			//	this.GameObject.transform.SetParent(Global.transform, false);
			//	this.GameObject.AddComponent<ComponentView>().Component = this;
			//}
#endif
		}


		public virtual void Dispose()
		{
			if (this.IsDisposed)
			{
				return;
			}

			Game.EventSystem.Remove(this.InstanceId);
			
			this.InstanceId = 0;

			if (this.IsFromPool)
			{
				Game.ObjectPool.Recycle(this);
			}
			else
			{
#if !SERVER
				if (this.GameObject != null)
				{
					UnityEngine.Object.Destroy(this.GameObject);
				}
#endif
			}
		}
		
		public override string ToString()
		{
			return MongoHelper.ToJson(this);
		}
	}
}