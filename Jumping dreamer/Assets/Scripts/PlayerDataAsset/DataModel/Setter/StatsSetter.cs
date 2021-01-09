using System;

public class StatsSetter : IStatsSetter, IStatsChangingNotifier
{
    private readonly PlayerStats1 playerStats;


    public StatsSetter(PlayerStats1 stats)
    {
        playerStats = stats;
    }


    public event Action OnNewScoreRecord;


    void IStatsSetter.SaveRecordStarsData(SafeInt starsAmount)
    {
        if (starsAmount > playerStats.RecordCollectedStars)
        {
            playerStats.RecordCollectedStars = starsAmount;
        }
    }


    void IStatsSetter.SaveScoreData(SafeInt scoreAmount)
    {
        if (scoreAmount > playerStats.RecordEarnedScore)
        {
            playerStats.RecordEarnedScore = scoreAmount;
            OnNewScoreRecord?.Invoke();
        }
    }


    void IStatsSetter.SaveScoreMultiplierData(SafeInt multiplierValue)
    {
        if (multiplierValue > playerStats.RecordScoreMultiplierValue)
        {
            playerStats.RecordScoreMultiplierValue = multiplierValue;
        }
    }


    void IStatsSetter.SaveLifeTimeData(SafeInt lifeTime)
    {
        playerStats.TotalLifeTime += lifeTime;

        if (lifeTime > playerStats.RecordLifeTime)
        {
            playerStats.RecordLifeTime = lifeTime;
        }
    }
}
