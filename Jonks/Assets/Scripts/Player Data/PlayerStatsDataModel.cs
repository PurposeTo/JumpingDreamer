using System;

[Serializable]
public class PlayerStatsDataModel
{
    // Лучшие результаты за все время игры
    public int? maxCollectedStars;
    public int? maxEarnedScore;
    public int? maxScoreMultiplierValue;
    public int? maxLifeTime;
    //public float? maxJumpHeight; // json хранит double

    // Общие результаты за все время игры
    public int? totalCollectedStars;
    public int? totalLifeTime;
}
