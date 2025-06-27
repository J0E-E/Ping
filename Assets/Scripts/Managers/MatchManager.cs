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
    
    private async void HandlePlayerButtonClicked(Player opponent)
    {
        try
        {
            Debug.Log($"MatchManager: Player {opponent.userName} clicked");
            // request matchmaking for the selected player
            await RequestMatchmaking(opponent);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Something went wrong requesting matchmaking: {ex.Message}");
        }
    }
    
    private async Task<bool> RequestMatchmaking(Player opponent)
    {
        Debug.Log($"Requesting matchmaking for player: {opponent.userName}");
        MatchMessage matchMessage = new RequestMatchMessage(opponent);
        return await _websocketManager.SendMessage(matchMessage);
    }
}
