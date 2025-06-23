using UnityEngine;

public class WSMessageRoutingManager : Manager
{
    public void RouteMessage(byte[] bytes)
    {
        HandleMessage(bytes);
    }
    
    private void HandleMessage(byte[] bytes)
    {
        var msg = System.Text.Encoding.UTF8.GetString(bytes);
        var baseMessage = JsonUtility.FromJson<BaseMessage>(msg);

        switch (baseMessage.type)
        {
            case "connect":
                var connectMessage = JsonUtility.FromJson<ConnectMessage>(msg);
                if (connectMessage.connectionId != null)
                {
                    SessionContext.ConnectionId = connectMessage.connectionId;
                }
                Debug.Log($"Connected. ConnectionId: {SessionContext.ConnectionId}");
                break;
            case "joinLobby":
                Debug.Log("Joining Lobby");
                break;
        }
    }
}
