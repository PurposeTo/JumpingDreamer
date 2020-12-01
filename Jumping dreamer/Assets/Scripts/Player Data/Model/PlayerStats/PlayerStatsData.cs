using Newtonsoft.Json;
using System.Collections.Generic;

public class PlayerStatsData : IGetStatsData
{
    // Лучшие результаты за все время игры
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? MaxCollectedStars { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? MaxEarnedScore { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? MaxScoreMultiplierValue { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? MaxLifeTime { get; set; }

    // Общие результаты за все время игры
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? TotalLifeTime { get; set; }



    public override bool Equals(object obj)
    {
        return obj is PlayerStatsData data &&
               EqualityComparer<SafeInt?>.Default.Equals(MaxCollectedStars, data.MaxCollectedStars) &&
               EqualityComparer<SafeInt?>.Default.Equals(MaxEarnedScore, data.MaxEarnedScore) &&
               EqualityComparer<SafeInt?>.Default.Equals(MaxScoreMultiplierValue, data.MaxScoreMultiplierValue) &&
               EqualityComparer<SafeInt?>.Default.Equals(MaxLifeTime, data.MaxLifeTime) &&
               EqualityComparer<SafeInt?>.Default.Equals(TotalLifeTime, data.TotalLifeTime);
    }


    public override int GetHashCode()
    {
        int hashCode = 502919464;
        hashCode = hashCode * -1521134295 + MaxCollectedStars.GetHashCode();
        hashCode = hashCode * -1521134295 + MaxEarnedScore.GetHashCode();
        hashCode = hashCode * -1521134295 + MaxScoreMultiplierValue.GetHashCode();
        hashCode = hashCode * -1521134295 + MaxLifeTime.GetHashCode();
        hashCode = hashCode * -1521134295 + TotalLifeTime.GetHashCode();

        return hashCode;
    }
}
