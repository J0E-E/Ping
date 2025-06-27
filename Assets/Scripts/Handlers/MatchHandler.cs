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
            case "match-started":
                Debug.Log($"Match started.");
                // Handle match start logic here
                break;
            case "update-match-state":
                Debug.Log($"Updating match state.");
                var stateUpdate = JsonConvert.DeserializeObject<MatchStateUpdate>(json);
                _matchManager.UpdateMatchState(stateUpdate);
                break;
            case "match-over":
                Debug.Log($"Match ended.");
                var matchOver = JsonConvert.DeserializeObject<MatchOver>(json);
                _matchManager.GameOver(matchOver.IsWinner);
                // Handle match end logic here
                break;
            default:
                Debug.LogWarning($"Unsupported Match message action: {message.action}");
                break;
        }
    }
}
