using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private string _playerName;

    public void UpdateName(string inputString)
    {
        _playerName = inputString;
    }

    public void EnterLobby()
    {
        Debug.Log($"{_playerName} Entering Lobby.");
    }
}
