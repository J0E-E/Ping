using System;

[Serializable]
public class BaseMessage
{
    // Base class for all messages sent over WebSocket
    public string type;
    public string action;
    public string connectionId;
    

    public BaseMessage()
    {
        this.connectionId = SessionContext.ConnectionId;
    }
}