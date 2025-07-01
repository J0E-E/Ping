using System;
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
public class AcceptMatch : MatchMessage
{
    public string playerId;

    public AcceptMatch(Player player)
    {
        this.action = "accept-match";
        this.playerId = player.playerId;
    }
}

[Serializable]
public class MatchStateMessage
{
    public PlayerType myPlayerType;
    public bool playerReady;
    public bool opponentReady;
    public Vector2 ballPosition;
    public Vector2 ballVelocity;
    public float playerPaddlePosition;
    public float opponentPaddlePosition;
    public int playerScore;
    public int opponentScore;
    public int winningScore;
    public GamePhase currentPhase;
}

[Serializable]
public class MatchStateUpdate : MatchMessage
{
    public MatchStateMessage matchState;
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
    public MatchStateMessage matchState;
}

[Serializable]
public class MatchRequested : MatchMessage
{
    public Player player;
}

[Serializable]
public class ReadyToStart : MatchMessage
{
    public bool isReady = true;
    public string matchId;
    public string playerType;

    public ReadyToStart(string matchId, PlayerType playerType)
    {
        this.action = "ready-to-play";
        this.matchId = matchId;
        this.playerType = playerType.ToString();
    }
}

[Serializable]
public class MovePaddle : MatchMessage
{
    public float xMovement;
    public string matchId;

    public MovePaddle(string matchId, float xMovement)
    {
        this.action = "move-paddle";
        this.matchId = matchId;
        this.xMovement = xMovement;
    }
}
