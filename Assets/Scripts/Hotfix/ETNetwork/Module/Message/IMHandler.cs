using System;
using ETModel;

namespace ETHotfix
{

	public interface IMHandler
	{
		void Handle(ETModel.Session session, object message);
		Type GetMessageType();
	}

}