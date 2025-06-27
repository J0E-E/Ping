using System;
using System.Collections.Generic;
using Newtonsoft.Json;

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
