using System;
using System.Threading.Tasks;
using NativeWebSocket;
using UnityEngine;

public enum ConnectionState
{
    Disconnected,
    Connected,
    LoggedIn,
    Reconnecting
}

public class WebsocketManager: Manager
{`
    private static WebSocket _webSocket;
    [SerializeField] private int maxRetries = 5;
    private static bool _isHandlingReconnect = false;

    private static ConnectionState _connectionState = ConnectionState.Disconnected;
    private static WSMessageRoutingManager Router => ManagerLocator.Get<WSMessageRoutingManager>();

    public static event Action<ConnectionState> ConnectionStateChange;

    private void Awake()
    {
        UpdateConnectionState(ConnectionState.Disconnected);
    }

    private void UpdateConnectionState(ConnectionState state)
    {
        _connectionState = state;
        ConnectionStateChange?.Invoke(state);
    }

    public static ConnectionState GetConnectionState()
    {
        return _connectionState;
    }

    private async void Start()
    {
        try
        {
            await ConnectToServer();
        }
        catch (Exception e)
        {
            if (_webSocket != null)
            {
                await _webSocket.Close();
            }
            UpdateConnectionState(ConnectionState.Disconnected);
            // TODO: Handle ConnectToServer catch better.
            Debug.LogError($"There was a problem connecting to the server: {e.Message}");
        }
    }

    private async Task<bool> ConnectToServer()
    {
        Debug.Log("Attempting WebSocket connection...");
        
        var connectedTcs = new TaskCompletionSource<bool>();

        if (_webSocket != null)
        {
            Debug.Log("Closing existing WebSocket...");
            await _webSocket.Close();
            _webSocket.OnOpen -= OnOpenHandler;
            _webSocket.OnMessage -= RouteMessage;
            _webSocket.OnError -= OnError;
            _webSocket.OnClose -= HandleCloseConnection;
            _webSocket = null;
        }

        var url = $"ws://localhost:3000?connectionId={SessionContext.ConnectionId}";
        Debug.Log($"Connecting to: {url}");

        _webSocket = new WebSocket(url);

        void OnOpenHandler()
        {
            Debug.Log("WebSocket connection established");
            _webSocket.OnOpen -= OnOpenHandler; // clean up
            connectedTcs.TrySetResult(true);
        }

        _webSocket.OnOpen += OnOpenHandler;
        _webSocket.OnMessage += RouteMessage;
        _webSocket.OnError += OnError;
        _webSocket.OnClose += HandleCloseConnection;

        await _webSocket.Connect();

        // Wait for OnOpen or timeout
        var completedTask = await Task.WhenAny(connectedTcs.Task, Task.Delay(5000));
        if (completedTask == connectedTcs.Task && connectedTcs.Task.Result)
        {
            UpdateConnectionState(ConnectionState.Connected);
            return true;
        }

        Debug.LogError("WebSocket connect timed out.");
        return false;
    }

    private static void OnError(string error)
    {
        Debug.Log("Error: " + error);
    }

    public async Task<bool> SendWsMessage<T>(T payload)
    {
        if (_webSocket == null || _webSocket.State != WebSocketState.Open)
        {
            Debug.LogWarning("WebSocket not ready to send message.");
            return false;
        }

        try
        {
            string json = JsonUtility.ToJson(payload);
            await _webSocket.SendText(json);
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
        if (_webSocket != null)
        {
            _webSocket.DispatchMessageQueue();
        }
    }

    private void RouteMessage(byte[] bytes)
    {
        Router.RouteMessage(bytes);
    }

    private async void HandleCloseConnection(WebSocketCloseCode closeCode)
    {
        try
        {
            if (closeCode == WebSocketCloseCode.Normal || _isHandlingReconnect) return;
            
            _isHandlingReconnect = true;
            UpdateConnectionState(ConnectionState.Reconnecting);
            Debug.LogError("Connection broken.");
            
            var retryCount = 0;
            bool connected = false;
            
            while (retryCount < maxRetries && !connected)
            {
                retryCount++;
                Debug.Log($"Reconnection attempt: {retryCount}");
                connected = await ConnectToServer();
                await Task.Delay(retryCount * 1000); // Backoff delay
            }

            if (connected)
            {
                Debug.Log("Reconnected to server.");
            }
            else
            {
                Debug.LogError("Failed to reconnect after max attempts.");
                UpdateConnectionState(ConnectionState.Disconnected);
            }

            _isHandlingReconnect = false;
        }
        catch (Exception e)
        {
            if (_webSocket != null)
            {
                await _webSocket.Close();
            }
            UpdateConnectionState(ConnectionState.Disconnected);
            Debug.LogError($"There was a problem reconnecting to the server: {e.Message}");
        }
        
    }
    
    private async void OnApplicationQuit()
    {
        if (_webSocket != null)
        {
            await _webSocket.Close();
        }
    }
}
