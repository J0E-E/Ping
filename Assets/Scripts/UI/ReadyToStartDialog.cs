using UnityEngine;

public class ReadyToStartDialog : Dialog
{
    private MatchManager _matchManager => ManagerLocator.Get<MatchManager>();
    public void OnReadyClicked()
    {
        _matchManager.ReadyToStartMatch();
        Hide();
    }

    public void OnCancelClicked()
    {
        Debug.Log("Canceling match.");
    }
}
