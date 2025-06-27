using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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

[Serializable]
public class Player
{
    // Represents a player in the lobby
    public string playerId;
    public string userName;
    public string playerLocation;
    public string rating;
}

[Serializable]
public class LobbyMessage: BaseMessage
{
    // This message is used for lobby-related actions
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string userName;    
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public Player playerData;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public List<Player> players;

    public LobbyMessage(string userName, string action)
    {
        // Constructor for creating a new LobbyMessage
        this.type = "lobby";
        this.action = action;
        this.userName = userName;
    }
}

[Serializable]
public class MatchMessage: BaseMessage
{
    // This message is used for match-related actions
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string playerId;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string opponentId;
    [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
    public string matchId;

    public MatchMessage(string action, Player self, Player opponent, string matchId = null)
    {
        // Constructor for creating a new MatchMessage
        this.type = "match";
        this.action = action;
        this.playerId = self.playerId;
        this.opponentId = opponent.playerId;
        this.matchId = matchId;
    }
}


