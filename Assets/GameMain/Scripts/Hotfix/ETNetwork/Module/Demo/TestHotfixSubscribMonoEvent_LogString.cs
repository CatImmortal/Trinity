using ETModel;

namespace Trinity.Hotfix
{
	// 分发数值监听
	[Event(ETModel.EventIdType.TestHotfixSubscribMonoEvent)]
	public class TestHotfixSubscribMonoEvent_LogString : AEvent<string>
	{
		public override void Run(string info)
		{
            ETModel.ETLog.Info(info);
		}
	}
}
