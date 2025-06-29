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

[Serializable]
public class RequestMatch : MatchMessage
{
    public string opponentId;
    
    public RequestMatch(Player opponent)
    {
        this.action = "request-match";
        this.opponentId = opponent.playerId;
    }
}

[Serializable]
public class UpdatePaddlePosition : MatchMessage
{
    public Vector2 paddlePosition;
    public UpdatePaddlePosition(Vector2 paddlePosition)
    {
        this.paddlePosition = paddlePosition;
    }
}

[Serializable]
public class MatchStateUpdate : MatchMessage
{
    public PlayerType myPlayerType;
    public bool playerReady;
    public bool opponentReady;
    public Vector2 ballPosition;
    public Vector2 ballVelocity;
    public int playerPaddlePosition;
    public int opponentPaddlePosition;
    public int playerScore;
    public int opponentScore;
    public int winningScore;
    public GamePhase currentPhase;
}


[Serializable]
public class MatchOver : MatchMessage
{
    public bool isWinner;
}

[Serializable]
public class MatchInitialized : MatchMessage
{
    public string matchId;
    public PlayerType playerType;
    public MatchStateUpdate matchState;
}
