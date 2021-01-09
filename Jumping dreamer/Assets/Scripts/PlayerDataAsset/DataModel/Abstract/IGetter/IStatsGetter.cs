public interface IStatsGetter
{
    // Лучшие результаты за все время игры
    public SafeInt? RecordCollectedStars { get; }
    public SafeInt? RecordEarnedScore { get; }
    public SafeInt? RecordScoreMultiplierValue { get; }
    public SafeInt? RecordLifeTime { get; }

    // Общие результаты за все время игры
    public SafeInt? TotalLifeTime { get; }
}
