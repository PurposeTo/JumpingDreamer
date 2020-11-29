public interface IGetStatsData
{
   public SafeInt? MaxCollectedStars { get; }
   public SafeInt? MaxEarnedScore { get; }
   public SafeInt? MaxScoreMultiplierValue { get; }
   public SafeInt? MaxLifeTime { get; }
   public SafeInt? TotalLifeTime { get; }
}
