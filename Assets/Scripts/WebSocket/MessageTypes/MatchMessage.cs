using System;
using Newtonsoft.Json;
using Unity.VisualScripting;

[Serializable]
public class MatchMessage: BaseMessage
{
    public MatchMessage()
    {
        this.type = "match";
    }
}

public class RequestMatchMessage : MatchMessage
{
    private string _opponentId;
    
    public RequestMatchMessage(Player opponent)
    {
        this.action = "request-match";
        this._opponentId = opponent.playerId;
    }
}
