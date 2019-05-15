using System;
using ILRuntime.CLR.Method;
using ILRuntime.Runtime.Enviorment;
using ILRuntime.Runtime.Intepreter;

namespace Trinity
{

	public class IDisposableAdaptor : CrossBindingAdaptor
	{
		public override Type BaseCLRType
		{
			get
			{
				return typeof (IDisposable);
			}
		}

		public override Type AdaptorType
		{
			get
			{
				return typeof (Adaptor);
			}
		}

		public override object CreateCLRInstance(ILRuntime.Runtime.Enviorment.AppDomain appdomain, ILTypeInstance instance)
		{
			return new Adaptor(appdomain, instance);
		}
		
		public class Adaptor: IDisposable, CrossBindingAdaptorType
		{
			private ILTypeInstance instance;
			private ILRuntime.Runtime.Enviorment.AppDomain appDomain;
			
			private IMethod iDisposable;
			private readonly object[] param0 = new object[0];

			public Adaptor()
			{
			}

			public Adaptor(ILRuntime.Runtime.Enviorment.AppDomain appDomain, ILTypeInstance instance)
			{
				this.appDomain = appDomain;
				this.instance = instance;
			}

			public ILTypeInstance ILInstance
			{
				get
				{
					return instance;
				}
			}

			public void Dispose()
			{
				if (this.iDisposable == null)
				{
					this.iDisposable = instance.Type.GetMethod("Dispose");
				}
				this.appDomain.Invoke(this.iDisposable, instance, this.param0);
			}

			public override string ToString()
			{
				IMethod m = this.appDomain.ObjectType.GetMethod("ToString", 0);
				m = instance.Type.GetVirtualMethod(m);
				if (m == null || m is ILMethod)
				{
					return instance.ToString();
				}

				return instance.Type.FullName;
			}


		}
	}
}
