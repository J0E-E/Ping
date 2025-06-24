using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public void OnConnection(string json)
    { 
        var message = JsonUtility.FromJson<ConnectionMessage>(json);
        Debug.Log("GameHandler - ConnectionId: " + message.connectionId);
    }
}
