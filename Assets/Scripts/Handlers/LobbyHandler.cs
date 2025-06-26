using System;
using System.Collections.Generic;
using UnityEngine;

public class LobbyHandler : MonoBehaviour
{
    public static event Action<List<Player>> OnUpdateLobby;
    
    public void OnLobby(string json)
    {
        var message = JsonUtility.FromJson<LobbyMessage>(json);
        switch (message.action)
        {
            case "logged-in":
                Debug.Log($"{message.playerData.userName} logged in successfully");
                SessionContext.PlayerData = message.playerData;
                break;
            case "update-players":
                OnUpdateLobby?.Invoke(message.players);
                break;
            default:
                Debug.LogWarning($"Unsupported Lobby message action");
                break;
        }
    }
}
