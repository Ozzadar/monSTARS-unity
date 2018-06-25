using System;

public interface IWebsocketMessageReceiver
{
	void ReceiveMessage<T>(T message);
	void ReceiveManager(WebsocketManager wsManager);
}


