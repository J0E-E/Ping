using Newtonsoft.Json;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public void OnConnectionMessage(string json)
    { 
        var message = JsonConvert.DeserializeObject<BaseMessage>(json);
        Debug.Log("GameHandler - ConnectionId: " + message.connectionId);
        SessionContext.ConnectionId = message.connectionId;
    }
}
