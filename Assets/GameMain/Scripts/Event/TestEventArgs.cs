using UnityEngine;
using GameFramework.Event;

//自动生成于：2019/4/13 22:40:36
namespace Trinity
{

	public class TestEventArgs : GameEventArgs
	{

		public static readonly int EventId = typeof(TestEventArgs).GetHashCode();

		public override int Id
		{
			get
			{
				return EventId;
			}
		}
		public float Hp
		{
			get;
			private set;
		}

		public int Atk
		{
			get;
			private set;
		}

		public Vector3 Pos
		{
			get;
			private set;
		}

		public override void Clear()
		{
			Hp = default(float);
			Atk = default(int);
			Pos = default(Vector3);
		}

		public TestEventArgs Fill(float hp,int atk,Vector3 pos)
		{
			Hp = hp;
			Atk = atk;
			Pos = pos;
			return this;
		}
	}
}
