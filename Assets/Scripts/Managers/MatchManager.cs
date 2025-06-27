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
        RequestMatch requestMatch = new RequestMatch(opponent);
        return await _websocketManager.SendMessage(requestMatch);
    }

    public void UpdateMatchState(MatchStateUpdate stateUpdate)
    {
        MatchState.MyPlayerType = stateUpdate.MyPlayerType;
        MatchState.PlayerReady = stateUpdate.PlayerReady;
        MatchState.OpponentReady = stateUpdate.OpponentReady;
        MatchState.BallPosition = stateUpdate.BallPosition;
        MatchState.BallVelocity = stateUpdate.BallVelocity;
        MatchState.PlayerPaddlePosition = stateUpdate.PlayerPaddlePosition;
        MatchState.OpponentPaddlePosition = stateUpdate.OpponentPaddlePosition;
        MatchState.PlayerScore = stateUpdate.PlayerScore;
        MatchState.OpponentScore = stateUpdate.OpponentScore;
        MatchState.WinningScore = stateUpdate.WinningScore;
        MatchState.CurrentPhase = stateUpdate.CurrentPhase;
        MatchState.LogState();
    }

    public void GameOver(bool isWinner)
    {
        Debug.Log(isWinner ? "WON GAME!" : "LOST GAME!");
    }
}