using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PingGameManager : GameManager
{
    private WebsocketManager _websocketManager => ManagerLocator.Get<WebsocketManager>();
    private WSMessageRoutingManager _wsMessageRoutingManager => ManagerLocator.Get<WSMessageRoutingManager>();

    [SerializeField] private GameObject paddlePrefab;
    private GameObject _playerPaddle;
    private GameObject _opponentPaddle;
    private bool _inMatch;
    private int _playerTypeModifier = 0;

    private void OnEnable()
    {
        MatchManager.MatchStateUpdated += OnMatchStateUpdated;
    }

    private void OnDisable()
    {
        MatchManager.MatchStateUpdated -= OnMatchStateUpdated;
    }

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

    private void OnMatchStateUpdated()
    {
        if (!_inMatch) _inMatch = true;
        
        Debug.Log("MatchState Updated.");
        
        if (_playerTypeModifier == 0)
        {
            _playerTypeModifier = MatchState.MyPlayerType == PlayerType.Player ? 1 : -1;
        }
        
        if (MatchState.PlayerReady && _playerPaddle == null)
        {
            LoadPlayerPaddle();
        }
        
        if (MatchState.OpponentReady && _opponentPaddle == null)
        {
            LoadOpponentPaddle();
        }

        if (_playerPaddle != null )
        {
            _playerPaddle.transform.position =
                new Vector2(MatchState.PlayerPaddlePosition * (MatchState.MyPlayerType == PlayerType.Player ? 1 : -1), _playerPaddle.transform.position.y);
        }
        if (_opponentPaddle != null)
        {
            _opponentPaddle.transform.position =
                new Vector2(MatchState.OpponentPaddlePosition * (MatchState.MyPlayerType == PlayerType.Opponent ? 1 : -1), _opponentPaddle.transform.position.y);
        }
    }

    private void LoadPlayerPaddle()
    {
        _playerPaddle = Instantiate(paddlePrefab);
        _playerPaddle.transform.position = new Vector2(0, -45 * _playerTypeModifier);
    }

    private void LoadOpponentPaddle()
    {
        _opponentPaddle = Instantiate(paddlePrefab);
        _opponentPaddle.transform.position = new Vector2(0, 45 * _playerTypeModifier);
    }

    private async void OnMove(InputValue inputValue)
    {
        if (!_inMatch) return;
        MovePaddle movePaddle = new MovePaddle(MatchState.MatchId, inputValue.Get<Vector2>().x);
        await _websocketManager.SendMessage(movePaddle);
    }
}

