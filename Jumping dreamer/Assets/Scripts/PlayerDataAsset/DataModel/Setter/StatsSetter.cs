using System;

public class StatsSetter : IStatsSetter, IStatsChangingNotifier
{
    private readonly PlayerStatsData playerStats;


    public StatsSetter(PlayerStatsData stats)
    {
        playerStats = stats;
    }


    public event Action OnNewScoreRecord;


    void IStatsSetter.SaveRecordStars(SafeInt starsAmount)
    {
        if (starsAmount > playerStats.RecordCollectedStars)
        {
            playerStats.RecordCollectedStars = starsAmount;
        }
    }


    void IStatsSetter.SaveRecordScore(SafeInt scoreAmount)
    {
        if (scoreAmount > playerStats.RecordEarnedScore)
        {
            playerStats.RecordEarnedScore = scoreAmount;
            OnNewScoreRecord?.Invoke();
        }
    }


    void IStatsSetter.SaveRecordScoreMultiplier(SafeInt multiplierValue)
    {
        if (multiplierValue > playerStats.RecordScoreMultiplierValue)
        {
            playerStats.RecordScoreMultiplierValue = multiplierValue;
        }
    }


    void IStatsSetter.SaveRecordLifeTime(SafeInt lifeTime)
    {
        playerStats.TotalLifeTime += lifeTime;

        if (lifeTime > playerStats.RecordLifeTime)
        {
            playerStats.RecordLifeTime = lifeTime;
        }
    }
}
