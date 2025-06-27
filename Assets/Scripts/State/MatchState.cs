using UnityEngine;

public static class MatchState
{
    public static bool PlayerReady = false;
    public static bool OpponentReady = false;
    public static Vector2 BallPosition = Vector2.zero;
    public static Vector2 BallVelocity = Vector2.zero;
    public static int PlayerPaddlePosition = 0;
    public static int OpponentPaddlePosition = 0;
    public static int PlayerScore = 0;
    public static int OpponentScore = 0;
    public static int WinningScore = 10;

    public enum GamePhase {
        WaitingForPlayers, 
        Countdown, 
        Playing,
        Paused,
        GameOver
    };

    public static GamePhase CurrentPhase = GamePhase.WaitingForPlayers;
}