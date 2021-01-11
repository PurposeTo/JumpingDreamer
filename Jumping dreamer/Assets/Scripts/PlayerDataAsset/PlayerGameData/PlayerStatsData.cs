using Newtonsoft.Json;
using System.Collections.Generic;

public class PlayerStatsData : IStatsGetter
{
    public PlayerStatsData() { }

    public PlayerStatsData(IStatsGetter dataGetter)
    {
        if (dataGetter == null) throw new System.ArgumentNullException(nameof(dataGetter));

        RecordCollectedStars = dataGetter.RecordCollectedStars;
        RecordEarnedScore = dataGetter.RecordEarnedScore;
        RecordScoreMultiplierValue = dataGetter.RecordScoreMultiplierValue;
        RecordLifeTime = dataGetter.RecordLifeTime;
        TotalLifeTime = dataGetter.TotalLifeTime;
    }



    // Лучшие результаты за все время игры
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? RecordCollectedStars { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? RecordEarnedScore { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? RecordScoreMultiplierValue { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? RecordLifeTime { get; set; }

    // Общие результаты за все время игры
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? TotalLifeTime { get; set; }


    public static PlayerStatsData CreateStatsWithDefaultValues()
    {
        return new PlayerStatsData
        {
            RecordCollectedStars = default(int),
            RecordEarnedScore = default(int),
            RecordScoreMultiplierValue = default(int),
            RecordLifeTime = default(int),
            TotalLifeTime = default(int)
        };
    }


    public override string ToString()
    {
        return $"{{\n{RecordCollectedStars},\n{RecordEarnedScore},\n{RecordScoreMultiplierValue},\n{RecordLifeTime},\n{TotalLifeTime}\n}}";
    }


    public override bool Equals(object obj)
    {
        return obj is PlayerStatsData data &&
               EqualityComparer<SafeInt?>.Default.Equals(RecordCollectedStars, data.RecordCollectedStars) &&
               EqualityComparer<SafeInt?>.Default.Equals(RecordEarnedScore, data.RecordEarnedScore) &&
               EqualityComparer<SafeInt?>.Default.Equals(RecordScoreMultiplierValue, data.RecordScoreMultiplierValue) &&
               EqualityComparer<SafeInt?>.Default.Equals(RecordLifeTime, data.RecordLifeTime) &&
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
