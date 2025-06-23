using System;
using System.Threading.Tasks;
using NativeWebSocket;
using UnityEngine;

public class WebsocketManager: Manager
{
    private WebSocket webSocket;
    private int _maxRetries = 5;
    private bool _isHandlingReconnect = false;
    private WSMessageRoutingManager _router => ManagerLocator.Get<WSMessageRoutingManager>();

    private async void Start()
    { 
        await ConnectToServer();
    }

    private async System.Threading.Tasks.Task<bool> ConnectToServer()
    {
        Debug.Log("Attempting WebSocket connection...");
        
        var connectedTcs = new System.Threading.Tasks.TaskCompletionSource<bool>();

        if (webSocket != null)
        {
            Debug.Log("Closing existing WebSocket...");
            await webSocket.Close();
            webSocket.OnOpen -= OnOpenHandler;
            webSocket.OnMessage -= RouteMessage;
            webSocket.OnError -= OnError;
            webSocket.OnClose -= HandleCloseConnection;
            webSocket = null;
        }

        var url = $"ws://localhost:3000?connectionId={SessionContext.ConnectionId}";
        Debug.Log($"Connecting to: {url}");

        webSocket = new WebSocket(url);

        void OnOpenHandler()
        {
            Debug.Log("WebSocket connection established");
            webSocket.OnOpen -= OnOpenHandler; // clean up
            connectedTcs.TrySetResult(true);
        }

        webSocket.OnOpen += OnOpenHandler;
        webSocket.OnMessage += RouteMessage;
        webSocket.OnError += OnError;
        webSocket.OnClose += HandleCloseConnection;

        webSocket.Connect();

        // Wait for OnOpen or timeout
        var completedTask = await System.Threading.Tasks.Task.WhenAny(connectedTcs.Task, System.Threading.Tasks.Task.Delay(5000));
        if (completedTask == connectedTcs.Task && connectedTcs.Task.Result)
        {
            return true;
        }

        Debug.LogError("WebSocket connect timed out.");
        return false;
    }

    private void OnOpen()
    {
        Debug.Log("Connection open!");
    }

    private void OnError(string error)
    {
        Debug.Log("Error: " + error);
    }

    public async System.Threading.Tasks.Task<bool> SendWsMessage<T>(T payload)
    {
        if (webSocket == null || webSocket.State != WebSocketState.Open)
        {
            Debug.LogWarning("WebSocket not ready to send message.");
            return false;
        }

        try
        {
            string json = JsonUtility.ToJson(payload);
            await webSocket.SendText(json);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"WebSocket send error: {e.Message}");
            return false;
        }
    }
    
    private void Update()
    {
        if (webSocket != null)
        {
            webSocket.DispatchMessageQueue();
        }
    }

    private void RouteMessage(byte[] bytes)
    {
        _router.RouteMessage(bytes);
    }

    private async void HandleCloseConnection(WebSocketCloseCode closeCode)
    {
        if (closeCode != WebSocketCloseCode.Normal)
        {
            if (_isHandlingReconnect) return;
            _isHandlingReconnect = true;
            Debug.LogError("Connection broken.");
            var retryCount = 0;
            bool connected = false;
            while (retryCount < _maxRetries && !connected)
            {
                retryCount++;
                Debug.Log($"Reconnection attempt: {retryCount}");
                connected = await ConnectToServer();
            }
            if (connected)
            {
                Debug.Log("Reconnected to server.");
            }
            else
            {
                Debug.LogError("Failed to reconnect after max attempts.");
            }
            _isHandlingReconnect = false;
        }
    }
}
