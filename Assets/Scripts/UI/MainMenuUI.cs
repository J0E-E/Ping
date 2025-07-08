using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    private string _playerName;
    [SerializeField] private Button _enterLobbyButton;
    private PingGameManager _pingGameManager => ManagerLocator.Get<PingGameManager>();

    public void UpdateName(string inputString)
    {
        _playerName = inputString;
        _enterLobbyButton.interactable = (_playerName.Length >= 4 && !_playerName.Contains(" "));
    }

    public async void EnterLobby()
    {
        Debug.Log($"{_playerName} Entering Lobby.");
        bool loginSuccess = await _pingGameManager.EnterLobby(_playerName);
        if (loginSuccess)
        {
            SceneManager.LoadScene("Lobby");
        }
        Debug.Log($"Login success: {loginSuccess}");
    }

    public void RequestMatch()
    {
        // This method can be used to request a match with another player
        Debug.Log("Requesting match...");
    }
}
