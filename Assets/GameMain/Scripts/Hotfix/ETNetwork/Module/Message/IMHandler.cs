using System;
using ETModel;

namespace Trinity.Hotfix
{
	public interface IMHandler
	{
		void Handle(ETModel.Session session, object message);
		Type GetMessageType();
	}
}