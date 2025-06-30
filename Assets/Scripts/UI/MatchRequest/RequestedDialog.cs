using TMPro;
using UnityEngine;

public class RequestedDialog : Dialog
{
    [SerializeField] private TextMeshProUGUI _displayTextUI;
    private Player _requestingPlayer;

    private MatchManager _matchManager => ManagerLocator.Get<MatchManager>();

    public void MatchRequested(Player player)
    {
        Debug.Log("RequestedDialog - MathRequested");
        _requestingPlayer = player;
        _displayTextUI.text = $"Ping match requested by: {_requestingPlayer.userName}.";
        Show();
    }

    public void OnAccept()
    {
        if (_requestingPlayer == null) return;
        Debug.Log($"Accepting match from: {_requestingPlayer.userName}");
        _matchManager.AcceptMatch(_requestingPlayer);
        CleanUpAndHide();
    }

    public void OnDecline()
    {
        Debug.Log($"Declining match from: {_requestingPlayer.userName}");
        CleanUpAndHide();
    }

    private void CleanUpAndHide()
    {
        _requestingPlayer = null;
        Hide();
    }
}
