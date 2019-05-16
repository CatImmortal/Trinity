using System;
using ETModel;

namespace ETHotfix
{
	public static class ExceptionHelper
	{
		public static string ToStr(this Exception exception)
		{
			return $"{exception.Data["StackTrace"]} \n\n {exception}";
		}
	}
}