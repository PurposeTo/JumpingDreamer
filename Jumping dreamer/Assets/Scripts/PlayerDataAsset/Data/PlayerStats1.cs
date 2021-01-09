using Newtonsoft.Json;
using System.Collections.Generic;

public class PlayerStats1 : IStatsGetter
{
    // Лучшие результаты за все время игры
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? RecordCollectedStars { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? RecordEarnedScore { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? RecordScoreMultiplierValue { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? RecordLifeTime { get; set; }

    // Общие результаты за все время игры
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? TotalLifeTime { get; set; }


    public override string ToString()
    {
        return RecordCollectedStars.ToString() +
            RecordEarnedScore.ToString() +
            RecordScoreMultiplierValue.ToString() +
            RecordLifeTime.ToString() +
            TotalLifeTime.ToString();
    }


    public override bool Equals(object obj)
    {
        return obj is PlayerStatsData data &&
               EqualityComparer<SafeInt?>.Default.Equals(RecordCollectedStars, data.MaxCollectedStars) &&
               EqualityComparer<SafeInt?>.Default.Equals(RecordEarnedScore, data.MaxEarnedScore) &&
               EqualityComparer<SafeInt?>.Default.Equals(RecordScoreMultiplierValue, data.MaxScoreMultiplierValue) &&
               EqualityComparer<SafeInt?>.Default.Equals(RecordLifeTime, data.MaxLifeTime) &&
               EqualityComparer<SafeInt?>.Default.Equals(TotalLifeTime, data.TotalLifeTime);
    }


    public override int GetHashCode()
    {
        int hashCode = 502919464;
        hashCode = hashCode * -1521134295 + RecordCollectedStars.GetHashCode();
        hashCode = hashCode * -1521134295 + RecordEarnedScore.GetHashCode();
        hashCode = hashCode * -1521134295 + RecordScoreMultiplierValue.GetHashCode();
        hashCode = hashCode * -1521134295 + RecordLifeTime.GetHashCode();
        hashCode = hashCode * -1521134295 + TotalLifeTime.GetHashCode();

        return hashCode;
    }
}
