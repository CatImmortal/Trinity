using ETModel;

namespace Trinity.Hotfix
{
	[MessageHandler]
	public class G2C_TestHotfixMessageHandler : AMHandler<G2C_TestHotfixMessage>
	{
		protected override void Run(ETModel.Session session, G2C_TestHotfixMessage message)
		{
            ETModel.ETLog.Debug(message.Info);
		}
	}
}