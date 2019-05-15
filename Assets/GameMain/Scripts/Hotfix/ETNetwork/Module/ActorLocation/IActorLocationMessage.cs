namespace Trinity.Hotfix
{
    /// <summary>
    /// Actor消息
    /// </summary>
	public interface IActorLocationMessage : IActorRequest
	{
	}

    /// <summary>
    /// ActorRPC消息请求
    /// </summary>
	public interface IActorLocationRequest : IActorRequest
	{
	}
	
    /// <summary>
    /// ActorRPC消息响应
    /// </summary>
	public interface IActorLocationResponse : IActorResponse
	{
	}
}