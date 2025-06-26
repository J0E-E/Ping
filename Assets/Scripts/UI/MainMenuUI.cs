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
        
        foreach (var player in players)
        {
            GameObject playerEntry = Instantiate(_playerPrefab, _lobbyPlayers.transform);
            
            var playerName = playerEntry.transform.Find("PlayerName")?.GetComponent<TextMeshProUGUI>();
            var playerRating = playerEntry.transform.Find("PlayerRating")?.GetComponent<TextMeshProUGUI>();
            var backgroundImage = playerEntry.transform.Find("Image")?.GetComponent<Image>();
            
            if (playerName != null && playerRating != null)
            {
                playerName.text = player.userName;
                playerRating.text = player.rating;
                
                if (backgroundImage != null && player.playerId == SessionContext.PlayerData.playerId)
                {
                    backgroundImage.color = Color.aquamarine;
                }
            }
        }
    }
}
