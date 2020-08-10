using System;

[Serializable]
public class PlayerStatsDataModel
{
    // Лучшие результаты за все время игры
    public SafeInt MaxCollectedStars { get; set; }
    public SafeInt MaxEarnedScore { get; set; }
    public SafeInt MaxScoreMultiplierValue { get; set; }
    public SafeInt MaxLifeTime { get; set; }
    //public float maxJumpHeight; // json хранит double

    // Общие результаты за все время игры
    public SafeInt TotalCollectedStars { get; set; }
    public SafeInt TotalLifeTime { get; set; }


    public static PlayerStatsDataModel CreateModelWithDefaultValues()
    {
        return new PlayerStatsDataModel
        {
            MaxCollectedStars = default(int),
            MaxEarnedScore = default(int),
            MaxScoreMultiplierValue = default(int),
            MaxLifeTime = default(int),
            TotalCollectedStars = default(int),
            TotalLifeTime = default(int),
        };
    }
}
