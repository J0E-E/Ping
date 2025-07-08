using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup lobbyPlayers;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject lobbyUI;
    private void Awake()
    {
        if (SessionContext.PlayerData != null)
        {
            UpdateLobby();
        }
    }
    private void OnEnable()
    {
        LobbyHandler.OnUpdateLobby += UpdateLobby;
    }

    private void OnDisable()
    {
        LobbyHandler.OnUpdateLobby -= UpdateLobby;
    }
    
    private void UpdateLobby(List<Player> players = null)
    {
        if (!lobbyUI.activeSelf) return;

        players ??= SessionContext.LobbyMembers;
        
        foreach (Transform child in lobbyPlayers.transform)
        {
            Destroy(child.gameObject);
        }
        
        if (players == null || players.Count == 0)
        {
            Debug.Log("No players in the lobby.");
            return;
        }
        
        Debug.Log($"Updating lobby with {players.Count} players.");
        
        foreach (var player in players)
        {
            GameObject playerEntry = Instantiate(playerPrefab, lobbyPlayers.transform);
            
            // Find the PlayerName and PlayerRating components
            var playerName = playerEntry.transform.Find("PlayerName")?.GetComponent<TextMeshProUGUI>();
            var playerRating = playerEntry.transform.Find("PlayerRating")?.GetComponent<TextMeshProUGUI>();
            LobbyPlayer lobbyPlayer = playerEntry.GetComponent<LobbyPlayer>();
            var button = playerEntry.GetComponent<Button>();
            
            if (playerName != null && playerRating != null)
            {
                // Set player name and rating
                playerName.text = player.userName;
                playerRating.text = $"{player.wins}/{player.losses}";
                lobbyPlayer.PlayerData = player;

                // Disable button for self
                if (player.playerId == SessionContext.PlayerData.playerId)
                {
                    button.interactable = false;
                }
            }
        }
    }
}
