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

    public void InitializeMatch(MatchInitialized initializedMatch)
    {
        var stateUpdate = initializedMatch.matchState;
        MatchState.SetPlayerType(initializedMatch.playerType);
        UpdateMatchState(stateUpdate);
        Debug.Log($"Match Initialized.");
    }

    public void UpdateMatchState(MatchStateUpdate stateUpdate)
    {
        MatchState.PlayerReady = stateUpdate.playerReady;
        MatchState.OpponentReady = stateUpdate.opponentReady;
        MatchState.BallPosition = stateUpdate.ballPosition;
        MatchState.BallVelocity = stateUpdate.ballVelocity;
        MatchState.PlayerPaddlePosition = stateUpdate.playerPaddlePosition;
        MatchState.OpponentPaddlePosition = stateUpdate.opponentPaddlePosition;
        MatchState.PlayerScore = stateUpdate.playerScore;
        MatchState.OpponentScore = stateUpdate.opponentScore;
        MatchState.WinningScore = stateUpdate.winningScore;
        MatchState.CurrentPhase = stateUpdate.currentPhase;
        MatchState.LogState();
    }

    public void GameOver(bool isWinner)
    {
        Debug.Log(isWinner ? "WON GAME!" : "LOST GAME!");
    }
}