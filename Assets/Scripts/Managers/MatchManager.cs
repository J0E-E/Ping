using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchManager : Manager
{
    private WebsocketManager _websocketManager => ManagerLocator.Get<WebsocketManager>();

    public static event Action OnMatchStateUpdated;
    public static event Action OnMatchStarted;
    public static event Action OnMatchEnded;

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

    public void MatchRequested(Player player)
    {
        
        var dialog = FindFirstObjectByType<RequestedDialog>(FindObjectsInactive.Include);
        if (dialog == null)
        {
            Debug.LogError("RequestedDialog object not found.");
        }
        dialog.MatchRequested(player);
    }

    public async Task<bool> AcceptMatch(Player player)
    {
        Debug.Log($"Accepting match with user: {player.userName}");
        AcceptMatch acceptMatch = new AcceptMatch(player);
        return await _websocketManager.SendMessage(acceptMatch);

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
        MatchState.MyPlayerType = initializedMatch.playerType;
        MatchState.MatchId = initializedMatch.matchId;
        UpdateMatchState(stateUpdate);
        Debug.Log($"Initializing Match. MatchId: {MatchState.MatchId}");
        OnMatchStarted?.Invoke();
        SceneManager.LoadScene("Match");
    }

    public async Task<bool> ReadyToStartMatch()
    {
        ReadyToStart readyMessage = new ReadyToStart(MatchState.MatchId, MatchState.MyPlayerType);
        return await _websocketManager.SendMessage(readyMessage);
    }

    public void UpdateMatchState(MatchStateMessage stateUpdate)
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
        MatchState.BallPossession = stateUpdate.ballPossession;
        MatchState.IsBallInPlay = stateUpdate.isBallInPlay;
        OnMatchStateUpdated?.Invoke();
    }

    public void MatchEnded(string matchId)
    {
        if (matchId != MatchState.MatchId)
        {
            Debug.LogError($"Match Ended message received with incorrect matchId: {matchId}");
            return;
        }
        Debug.Log("Match Ended");
        OnMatchEnded?.Invoke();
        SceneManager.LoadScene("Lobby");
    }

    public void EndMatch()
    {
        var endMatchMessage = new EndMatch();
        _websocketManager.SendMessage(endMatchMessage);
    }
}