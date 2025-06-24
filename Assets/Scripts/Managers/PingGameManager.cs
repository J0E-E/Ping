using UnityEngine;

public class PingGameManager : GameManager
{
    private WebsocketManager _websocketManager => ManagerLocator.Get<WebsocketManager>();
    private WSMessageRoutingManager _wsMessageRoutingManager => ManagerLocator.Get<WSMessageRoutingManager>();
    
    protected override void Awake()
    {
        base.Awake();
        
        _wsMessageRoutingManager.RegisterHandlerScript("GameHandler", GetComponent<GameHandler>());
        
        _wsMessageRoutingManager.InitializeHandlers();
        
        _websocketManager.ConnectToWebSocketServer();
    }

}
