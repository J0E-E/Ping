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
public class UpdatePaddlePosition : MatchMessage
{
    public Vector2 paddlePosition;
    public UpdatePaddlePosition(Vector2 paddlePosition)
    {
        this.paddlePosition = paddlePosition;
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
    public int playerPaddlePosition;
    public int opponentPaddlePosition;
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
