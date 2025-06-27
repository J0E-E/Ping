using UnityEngine;

public class MatchHandler : MonoBehaviour
{
    public void OnMatch(string json)
    {
        var message = JsonUtility.FromJson<BaseMessage>(json);
        Debug.Log($"MatchHandler: Received match message with action '{message.action}'");

        switch (message.action)
        {
            case "match-started":
                Debug.Log($"Match started.");
                // Handle match start logic here
                break;
            case "match-ended":
                Debug.Log($"Match ended.");
                // Handle match end logic here
                break;
            default:
                Debug.LogWarning($"Unsupported Match message action: {message.action}");
                break;
        }
    }
}
