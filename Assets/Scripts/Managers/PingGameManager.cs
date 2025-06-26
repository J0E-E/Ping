using System.Threading.Tasks;
using UnityEngine;

public class PingGameManager : GameManager
{
    private WebsocketManager _websocketManager => ManagerLocator.Get<WebsocketManager>();
    private WSMessageRoutingManager _wsMessageRoutingManager => ManagerLocator.Get<WSMessageRoutingManager>();
    
    protected override void Awake()
    {
        base.Awake();
        
        _wsMessageRoutingManager.InitializeHandlers();
        
        _websocketManager.ConnectWebSocket();
    }

    protected override void RegisterSelf()
    {
        ManagerLocator.Register<PingGameManager>(this);
    }

    public async Task<bool> EnterLobby(string userName)
    {
        LobbyMessage lobbyMessage = new LobbyMessage(userName, "joining");
        
        return await _websocketManager.SendMessage(lobbyMessage);
    }
}
