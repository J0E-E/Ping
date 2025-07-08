using System.Collections.Generic;

public static class SessionContext
{
    public static string ConnectionId { get; set; }
    public static Player PlayerData { get; set; }
    public static Player OpponentData { get; set; }
    public static List<Player> LobbyMembers { get; set; }
}