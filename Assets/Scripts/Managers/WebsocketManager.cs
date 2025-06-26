using System;
using System.Threading.Tasks;
using NativeWebSocket;
using Newtonsoft.Json;
using UnityEngine;

public enum ConnectionState
{
    Disconnected,
    Connected,
    LoggedIn,
    Reconnecting
}

public class WebsocketManager : Manager
{
    private WebSocket _webSocket;
    private string URL => $"ws://localhost:3000?connectionId={SessionContext.ConnectionId}";
    private bool _isReconnecting = false;

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

    public static ConnectionState GetConnectionState() => _connectionState;

    public async void ConnectWebSocket()
    {
        _webSocket = new WebSocket(URL);

        _webSocket.OnOpen += () =>
        {
            Debug.Log("WebSocket connected!");
            UpdateConnectionState(ConnectionState.Connected);
        };

        _webSocket.OnClose += (e) =>
        {
            Debug.Log($"WebSocket disconnected with code: {e}");
            UpdateConnectionState(ConnectionState.Reconnecting);
            ReconnectWebSocket();
        };

        _webSocket.OnError += (e) => { Debug.LogError($"WebSocket error: {e}"); };

        _webSocket.OnMessage += (bytes) => { Router.RouteMessage(bytes); };

        try
        {
            await _webSocket.Connect();
        }
        catch (Exception ex)
        {
            Debug.LogError($"Initial WebSocket connection failed: {ex.Message}");
            ReconnectWebSocket();
        }
    }

    private void ReconnectWebSocket()
    {
        if (!_isReconnecting)
        {
            _isReconnecting = true;
            Invoke("AttemptReconnect", 5f);
        }
    }

    private void AttemptReconnect()
    {
        if (_webSocket == null || _webSocket.State == WebSocketState.Closed)
        {
            Debug.Log("Attempting to reconnect WebSocket...");
            ConnectWebSocket();
            _isReconnecting = false;
        }
    }

    public async Task<bool> SendMessage<T>(T payload)
    {
        if (_webSocket == null || _webSocket.State != WebSocketState.Open)
        {
            Debug.LogWarning("WebSocket not ready to send message.");
            return false;
        }

        try
        {
            string json = JsonConvert.SerializeObject(payload);
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
        _webSocket?.DispatchMessageQueue();
    }

    private async void OnApplicationQuit()
    {
        if (_webSocket != null)
        {
            await _webSocket.Close();
        }
    }
}