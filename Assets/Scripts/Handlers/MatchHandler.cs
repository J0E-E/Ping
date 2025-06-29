using Newtonsoft.Json;
using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    private MatchManager _matchManager => ManagerLocator.Get<MatchManager>();
    public void OnMatchMessage(string json)
    {
        var message = JsonUtility.FromJson<MatchMessage>(json);
        Debug.Log($"MatchHandler: Received match message with action '{message.action}'");

        switch (message.action)
        {
            case "match-initialized":
                Debug.Log($"Initializing Match.");
                var matchInitializedMessage = JsonConvert.DeserializeObject<MatchInitialized>(json);
                _matchManager.InitializeMatch(matchInitializedMessage);
                break;
            case "update-match-state":
                Debug.Log($"Updating match state.");
                var stateUpdate = JsonConvert.DeserializeObject<MatchStateUpdate>(json);
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
