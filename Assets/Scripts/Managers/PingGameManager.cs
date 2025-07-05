using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PingGameManager : GameManager
{
    private WebsocketManager _websocketManager => ManagerLocator.Get<WebsocketManager>();
    private WSMessageRoutingManager _wsMessageRoutingManager => ManagerLocator.Get<WSMessageRoutingManager>();

    [SerializeField] private GameObject paddlePrefab;
    [SerializeField] private GameObject ballPrefab;
    private GameObject _playerPaddle;
    private GameObject _opponentPaddle;
    private GameObject _ball;
    private bool _inMatch;
    private int _playerTypeModifier = 0;
    private bool _isTouching = false;
    private bool _isMovingRight = false;
    private bool _isMovingLeft = false;

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

        if (_ball == null && MatchState.PlayerReady && MatchState.OpponentReady && MatchState.BallPosition != Vector2.zero)
        {
            LoadBall();
        }
        else if (_ball != null && new Vector2(_ball.transform.position.x, _ball.transform.position.x) != MatchState.BallPosition)
        {
            UpdateBallPosition();
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

    private void LoadBall()
    {
        _ball = Instantiate(ballPrefab);
        _ball.transform.position = new Vector2(MatchState.BallPosition.x, MatchState.BallPosition.y * _playerTypeModifier);
    }

    private void UpdateBallPosition()
    {
        _ball.transform.position = new Vector2(MatchState.BallPosition.x * _playerTypeModifier, MatchState.BallPosition.y * _playerTypeModifier);
    }

    private async void OnMove(InputValue inputValue)
    {
        if (!_inMatch) return;
        Debug.Log(inputValue.Get<Vector2>());
        MovePaddle movePaddleMessage = new MovePaddle(MatchState.MatchId, inputValue.Get<Vector2>().x);
        await _websocketManager.SendMessage(movePaddleMessage);
    }

    private async void OnRelease(InputValue inputValue)
    {
        if (MatchState.IsBallInPlay || !MatchState.PlayersReady || MatchState.BallPossession != MatchState.MyPlayerType) return;
        
        ReleaseBall releaseBallMessage = new ReleaseBall();
        await _websocketManager.SendMessage(releaseBallMessage);
    }

    private async void OnTouch(InputValue inputValue)
    {
        if (inputValue == null) return;
        
        switch (inputValue.Get().ToString())
        {
            case "Ended":
                _isTouching = false;
                _isMovingRight = false;
                _isMovingLeft = false;
                MovePaddle movePaddleMessage = new MovePaddle(MatchState.MatchId, 0);
                await _websocketManager.SendMessage(movePaddleMessage);
                Debug.Log("Stopping.");
                break;
            case "Began":
                Debug.Log("Touch Began");
                _isTouching = true;
                break;
        }
    }

    private async void OnTouch2(InputValue inputValue)
    {
        if (inputValue == null) return;
        
        switch (inputValue.Get().ToString())
        {
            case "Ended":
                break;
            case "Began":
                if (MatchState.IsBallInPlay || !MatchState.PlayersReady || MatchState.BallPossession != MatchState.MyPlayerType) return;
        
                ReleaseBall releaseBallMessage = new ReleaseBall();
                await _websocketManager.SendMessage(releaseBallMessage);
                break;
        }
    }

    private async void OnSwipeLeft(InputValue inputValue)
    {
        if (inputValue == null) return;
     
        if (inputValue.Get<float>() > 0 && _isTouching && !_isMovingLeft)
        {
            MovePaddle movePaddleMessage = new MovePaddle(MatchState.MatchId, -1);
            await _websocketManager.SendMessage(movePaddleMessage);
            Debug.Log("Moving Left.");
            _isMovingRight = false;
            _isMovingLeft = true;
        } 
    }

    private async void OnSwipeRight(InputValue inputValue)
    {
        if (inputValue.Get<float>() > 0 && _isTouching && !_isMovingRight)
        {
            MovePaddle movePaddleMessage = new MovePaddle(MatchState.MatchId, 1);
            await _websocketManager.SendMessage(movePaddleMessage);
            Debug.Log("Moving Right.");
            _isMovingLeft = false;
            _isMovingRight = true;
        } 
    }
}

