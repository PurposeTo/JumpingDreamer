using System;

public interface IGetStatsData
{
    event Action OnNewScoreRecord;


    SafeInt? MaxCollectedStars { get; }
    SafeInt? MaxEarnedScore { get; }
    SafeInt? MaxScoreMultiplierValue { get; }
    SafeInt? MaxLifeTime { get; }
    SafeInt? TotalLifeTime { get; }
}
