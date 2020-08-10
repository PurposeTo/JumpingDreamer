using System;
using Newtonsoft.Json;

[Serializable]
public class PlayerStatsDataModel
{
    // Лучшие результаты за все время игры
    [JsonConverter(typeof(SafeIntConverter))]
    public SafeInt MaxCollectedStars { get; set; }
    [JsonConverter(typeof(SafeIntConverter))]
    public SafeInt MaxEarnedScore { get; set; }
    [JsonConverter(typeof(SafeIntConverter))]
    public SafeInt MaxScoreMultiplierValue { get; set; }
    [JsonConverter(typeof(SafeIntConverter))]
    public SafeInt MaxLifeTime { get; set; }
    //public float maxJumpHeight; // json хранит double

    // Общие результаты за все время игры
    [JsonConverter(typeof(SafeIntConverter))]
    public SafeInt TotalCollectedStars { get; set; }
    [JsonConverter(typeof(SafeIntConverter))]
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
