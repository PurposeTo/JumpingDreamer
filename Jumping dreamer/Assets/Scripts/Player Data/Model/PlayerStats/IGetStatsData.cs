public interface IGetStatsData
{
    SafeInt? MaxCollectedStars { get; }
    SafeInt? MaxEarnedScore { get; }
    SafeInt? MaxScoreMultiplierValue { get; }
    SafeInt? MaxLifeTime { get; }
    SafeInt? TotalLifeTime { get; }
}
