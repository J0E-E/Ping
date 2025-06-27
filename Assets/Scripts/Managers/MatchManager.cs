using System;
using System.Threading.Tasks;
using UnityEngine;

public class MatchManager : Manager
{
    private WebsocketManager _websocketManager => ManagerLocator.Get<WebsocketManager>();
    
    private void OnEnable()
    {
        LobbyPlayer.OnPlayerButtonClicked += HandlePlayerButtonClicked;
    }
    
    private void OnDisable()
    {
        LobbyPlayer.OnPlayerButtonClicked -= HandlePlayerButtonClicked;
    }
    
    private void HandlePlayerButtonClicked(Player opponent)
    {
        Debug.Log($"MatchManager: Player {opponent.userName} clicked");
        // request matchmaking for the selected player
        RequestMatchmaking(opponent);
    }
    
    private async Task<bool> RequestMatchmaking(Player opponent)
    {
        Debug.Log($"Requesting matchmaking for player: {opponent.userName}");
        MatchMessage matchMessage = new MatchMessage("request-match", SessionContext.PlayerData, opponent);
        return await _websocketManager.SendMessage(matchMessage);
    }
}
