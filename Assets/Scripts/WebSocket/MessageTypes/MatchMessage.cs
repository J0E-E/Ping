using System;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class MatchMessage: BaseMessage
{
    public MatchMessage()
    {
        this.type = "match";
    }
}

public class RequestMatch : MatchMessage
{
    public string OpponentId;
    
    public RequestMatch(Player opponent)
    {
        this.action = "request-match";
        this.OpponentId = opponent.playerId;
    }
}

public class UpdatePaddlePosition : MatchMessage
{
    public Vector2 PaddlePosition;
    public UpdatePaddlePosition(Vector2 paddlePosition)
    {
        this.PaddlePosition = paddlePosition;
    }
}

public class MatchStateUpdate : MatchMessage
{
    public PlayerType MyPlayerType;
    public bool PlayerReady;
    public bool OpponentReady;
    public Vector2 BallPosition;
    public Vector2 BallVelocity;
    public int PlayerPaddlePosition;
    public int OpponentPaddlePosition;
    public int PlayerScore;
    public int OpponentScore;
    public int WinningScore;
    public GamePhase CurrentPhase;
}

public class MatchOver : MatchMessage
{
    public bool IsWinner;
}
