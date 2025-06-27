using System;
using UnityEngine;

public class LobbyPlayer : MonoBehaviour
{
    public Player PlayerData { get; set; }
    
    public static event Action<Player> OnPlayerButtonClicked;

    public void OnClick()
    {
        if (PlayerData != null)
        {
            Debug.Log($"LobbyPlayer: Player {PlayerData.userName} clicked");
            OnPlayerButtonClicked?.Invoke(PlayerData);
        }
        else
        {
            Debug.LogWarning("PlayerData is null, cannot invoke OnPlayerButtonClicked");
        }
    }
}
