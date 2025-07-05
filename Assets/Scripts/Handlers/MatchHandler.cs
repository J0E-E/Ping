using Newtonsoft.Json;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    private MatchManager _matchManager => ManagerLocator.Get<MatchManager>();
    public void OnMatchMessage(string json)
    {
        var message = JsonConvert.DeserializeObject<MatchMessage>(json);

        switch (message.action)
        {
            case "match-requested":
                var matchRequestedMessage = JsonConvert.DeserializeObject<MatchRequested>(json);
                _matchManager.MatchRequested(matchRequestedMessage.player);
                break;
            case "match-accepted":
                var matchInitializedMessage = JsonConvert.DeserializeObject<MatchInitialized>(json);
                _matchManager.InitializeMatch(matchInitializedMessage);
                break;
            case "update-match-state":
                var stateUpdateMessage = JsonConvert.DeserializeObject<MatchStateUpdate>(json);
                var stateUpdate = stateUpdateMessage.matchState;
                _matchManager.UpdateMatchState(stateUpdate);
                break;
            case "match-over":
                Debug.Log($"Match ended.");
                var matchOver = JsonConvert.DeserializeObject<MatchOver>(json);
                _matchManager.GameOver(matchOver.isWinner);
                // Handle match end logic here
                break;
            default:
                Debug.LogWarning($"Unsupported Match message action: {message.action}");
                break;
        }
    }
}
