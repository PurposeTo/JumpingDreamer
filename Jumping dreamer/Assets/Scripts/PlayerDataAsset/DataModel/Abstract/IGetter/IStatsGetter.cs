public interface IStatsGetter
{
    // Лучшие результаты за все время игры
    SafeInt? RecordCollectedStars { get; }
    SafeInt? RecordEarnedScore { get; }
    SafeInt? RecordScoreMultiplierValue { get; }
    SafeInt? RecordLifeTime { get; }

    // Общие результаты за все время игры
    SafeInt? TotalLifeTime { get; }
}
