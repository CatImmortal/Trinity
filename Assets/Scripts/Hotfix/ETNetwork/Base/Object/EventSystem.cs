using System;
using System.Collections.Generic;
using ETModel;

namespace ETHotfix
{
	public sealed class EventSystem
	{
		private readonly Dictionary<long, Component> allComponents = new Dictionary<long, Component>();
		
		private readonly List<Type> types = new List<Type>();

		private readonly UnOrderMultiMap<Type, IAwakeSystem> awakeSystems = new UnOrderMultiMap<Type, IAwakeSystem>();

		private readonly UnOrderMultiMap<Type, ILoadSystem> loadSystems = new UnOrderMultiMap<Type, ILoadSystem>();

	
		private Queue<long> loaders = new Queue<long>();
		private Queue<long> loaders2 = new Queue<long>();


		public EventSystem()
		{
			this.types.Clear();
			
            //TODO:此处修改了ET源码
			//List<Type> ts = ETModel.Game.Hotfix.GetHotfixTypes();
            List<Type> ts = Trinity.GameEntry.ILRuntime.GetHotfixTypes();
            foreach (Type type in ts)
			{
				// ILRuntime无法判断是否有Attribute
				//if (type.GetCustomAttributes(typeof (Attribute), false).Length == 0)
				//{
				//	continue;
				//}
				
				this.types.Add(type);	
			}
			
			foreach (Type type in types)
			{
				object[] attrs = type.GetCustomAttributes(typeof(ObjectSystemAttribute), false);

				if (attrs.Length == 0)
				{
					continue;
				}

				object obj = Activator.CreateInstance(type);

				switch (obj)
				{
					case IAwakeSystem objectSystem:
						this.awakeSystems.Add(objectSystem.Type(), objectSystem);
						break;
					
					case ILoadSystem loadSystem:
						this.loadSystems.Add(loadSystem.Type(), loadSystem);
						break;
				
				}
			}

			this.Load();
		}

	
		public List<Type> GetTypes()
		{
			return this.types;
		}

		public void Add(Component component)
		{
			this.allComponents.Add(component.InstanceId, component);

			Type type = component.GetType();

			if (this.loadSystems.ContainsKey(type))
			{
				this.loaders.Enqueue(component.InstanceId);
			}

		
		}

		public void Remove(long instanceId)
		{
			this.allComponents.Remove(instanceId);
		}
	
		public void Awake(Component component)
		{
			List<IAwakeSystem> iAwakeSystems = this.awakeSystems[component.GetType()];
			if (iAwakeSystems == null)
			{
				return;
			}

			foreach (IAwakeSystem aAwakeSystem in iAwakeSystems)
			{
				if (aAwakeSystem == null)
				{
					continue;
				}
				
				IAwake iAwake = aAwakeSystem as IAwake;
				if (iAwake == null)
				{
					continue;
				}

				try
				{
					iAwake.Run(component);
				}
				catch (Exception e)
				{
					Log.Error(e);
				}
			}
		}

		public void Awake<P1>(Component component, P1 p1)
		{
			List<IAwakeSystem> iAwakeSystems = this.awakeSystems[component.GetType()];
			if (iAwakeSystems == null)
			{
				return;
			}
			
			foreach (IAwakeSystem iAwakeSystem in iAwakeSystems)
			{
				if (iAwakeSystem == null)
				{
					continue;
				}
				
				IAwake<P1> iAwake = iAwakeSystem as IAwake<P1>;
				if (iAwake == null)
				{
					continue;
				}

				try
				{
					iAwake.Run(component, p1);
				}
				catch (Exception e)
				{
					Log.Error(e);
				}
			}
		}

		public void Awake<P1, P2>(Component component, P1 p1, P2 p2)
		{
			List<IAwakeSystem> iAwakeSystems = this.awakeSystems[component.GetType()];
			if (iAwakeSystems == null)
			{
				return;
			}

			foreach (IAwakeSystem iAwakeSystem in iAwakeSystems)
			{
				if (iAwakeSystem == null)
				{
					continue;
				}
				
				IAwake<P1, P2> iAwake = iAwakeSystem as IAwake<P1, P2>;
				if (iAwake == null)
				{
					continue;
				}

				try
				{
					iAwake.Run(component, p1, p2);
				}
				catch (Exception e)
				{
					Log.Error(e);
				}
			}
		}

		public void Awake<P1, P2, P3>(Component component, P1 p1, P2 p2, P3 p3)
		{
			List<IAwakeSystem> iAwakeSystems = this.awakeSystems[component.GetType()];
			if (iAwakeSystems == null)
			{
				return;
			}

			foreach (IAwakeSystem iAwakeSystem in iAwakeSystems)
			{
				if (iAwakeSystem == null)
				{
					continue;
				}
				
				IAwake<P1, P2, P3> iAwake = iAwakeSystem as IAwake<P1, P2, P3>;
				if (iAwake == null)
				{
					continue;
				}

				try
				{
					iAwake.Run(component, p1, p2, p3);
				}
				catch (Exception e)
				{
					Log.Error(e);
				}
			}
		}

		public void Load()
		{
			while (this.loaders.Count > 0)
			{
				long instanceId = this.loaders.Dequeue();
				Component component;
				if (!this.allComponents.TryGetValue(instanceId, out component))
				{
					continue;
				}
				if (component.IsDisposed)
				{
					continue;
				}
				
				List<ILoadSystem> iLoadSystems = this.loadSystems[component.GetType()];
				if (iLoadSystems == null)
				{
					continue;
				}

				this.loaders2.Enqueue(instanceId);

				foreach (ILoadSystem iLoadSystem in iLoadSystems)
				{
					try
					{
						iLoadSystem.Run(component);
					}
					catch (Exception e)
					{
						Log.Error(e);
					}
				}
			}

			ObjectHelper.Swap(ref this.loaders, ref this.loaders2);
		}

	


	

		

	
		
	}
}