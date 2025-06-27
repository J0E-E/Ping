using System;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public void OnConnectionMessage(string json)
    { 
        var message = JsonUtility.FromJson<BaseMessage>(json);
        Debug.Log("GameHandler - ConnectionId: " + message.connectionId);
        SessionContext.ConnectionId = message.connectionId;
    }
}
