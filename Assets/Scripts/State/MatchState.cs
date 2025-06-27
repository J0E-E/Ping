using UnityEngine;

public enum PlayerType
{
    Player,
    Opponent
}

public enum GamePhase
{
    WaitingForPlayers,
    Countdown,
    Playing,
    Paused,
    GameOver
};

public static class MatchState
{
    public static PlayerType MyPlayerType;
    public static bool PlayerReady = false;
    public static bool OpponentReady = false;
    public static Vector2 BallPosition = Vector2.zero;
    public static Vector2 BallVelocity = Vector2.zero;
    public static float PlayerPaddlePosition = 0f;
    public static float OpponentPaddlePosition = 0f;
    public static int PlayerScore = 0;
    public static int OpponentScore = 0;
    public static int WinningScore = 10;

    public static GamePhase CurrentPhase = GamePhase.WaitingForPlayers;

    public static int MyScore => MyPlayerType == PlayerType.Player ? PlayerScore : OpponentScore;
    public static int TheirScore => MyPlayerType == PlayerType.Player ? OpponentScore : PlayerScore;

    public static float MyPaddlePosition =>
        MyPlayerType == PlayerType.Player ? PlayerPaddlePosition : OpponentPaddlePosition;

    public static float TheirPaddlePosition =>
        MyPlayerType == PlayerType.Player ? OpponentPaddlePosition : PlayerPaddlePosition;

    public static void LogState()
    {
        var currentState =
            $"MatchState: Phase={CurrentPhase}, PlayerReady={PlayerReady}, OpponentReady={OpponentReady}, " +
            $"BallPosition={BallPosition}, BallVelocity={BallVelocity}, PlayerPaddlePOS={PlayerPaddlePosition}, " +
            $"OpponentPaddlePOS={OpponentPaddlePosition}, PlayerScore={PlayerScore}, OpponentScore={OpponentScore}, " +
            $"WinningScore={WinningScore}, PlayerType={MyPlayerType}";
        Debug.Log(currentState);
    }
}