using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private string _playerName;
    [SerializeField] private Button _enterLobbyButton;
    [SerializeField] private GridLayoutGroup _lobbyPlayers;
    [SerializeField] private GameObject _playerPrefab;

    private PingGameManager _pingGameManager => ManagerLocator.Get<PingGameManager>();

    private void OnEnable()
    {
        LobbyHandler.OnUpdateLobby += UpdateLobby;
    }

    private void OnDisable()
    {
        LobbyHandler.OnUpdateLobby -= UpdateLobby;
    }

    public void UpdateName(string inputString)
    {
        _playerName = inputString;
        _enterLobbyButton.interactable = (_playerName.Length >= 4 && !_playerName.Contains(" "));
    }

    public async void EnterLobby()
    {
        Debug.Log($"{_playerName} Entering Lobby.");
        bool loginSuccess = await _pingGameManager.EnterLobby(_playerName);
        Debug.Log($"Login success: {loginSuccess}");
    }

    private void UpdateLobby(List<Player> players)
    {
        foreach (Transform child in _lobbyPlayers.transform)
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
            GameObject playerEntry = Instantiate(_playerPrefab, _lobbyPlayers.transform);
            
            // Find the PlayerName and PlayerRating components
            var playerName = playerEntry.transform.Find("PlayerName")?.GetComponent<TextMeshProUGUI>();
            var playerRating = playerEntry.transform.Find("PlayerRating")?.GetComponent<TextMeshProUGUI>();
            LobbyPlayer lobbyPlayer = playerEntry.GetComponent<LobbyPlayer>();
            var button = playerEntry.GetComponent<Button>();
            
            if (playerName != null && playerRating != null)
            {
                // Set player name and rating
                playerName.text = player.userName;
                playerRating.text = player.rating;
                lobbyPlayer.PlayerData = player;

                // Disable button for self
                if (player.playerId == SessionContext.PlayerData.playerId)
                {
                    button.interactable = false;
                }
            }
        }
    }

    public void RequestMatch()
    {
        // This method can be used to request a match with another player
        Debug.Log("Requesting match...");
    }
}
