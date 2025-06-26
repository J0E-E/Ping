using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class BaseMessage
{
    public string type;
    public string action;
    public string connectionId;
    

    public BaseMessage()
    {
        this.connectionId = SessionContext.ConnectionId;
    }
}

[Serializable]
public class Player
{
    public string playerId;
    public string userName;
    public string playerLocation;
    public string rating;
}

[Serializable]
public class LobbyMessage: BaseMessage
{
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string userName;    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Player playerData;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<Player> players;

    public LobbyMessage(string userName, string action)
    {
        this.type = "lobby";
        this.action = action;
        this.userName = userName;
    }
}


