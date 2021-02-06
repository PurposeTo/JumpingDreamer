using System;
using UnityEngine;

public class UsedStatsGetter : IStatsGetter
{
    private readonly IStatsGetter currentSessionData;
    private readonly IStatsGetter lastSessionsData;

    public UsedStatsGetter(IStatsGetter currentSessionData, IStatsGetter lastSessionsData)
    {
        this.currentSessionData = currentSessionData ?? throw new ArgumentNullException(nameof(currentSessionData));
        this.lastSessionsData = lastSessionsData ?? throw new ArgumentNullException(nameof(lastSessionsData));
    }


    SafeInt? IStatsGetter.RecordCollectedStars => Mathf.Max((int)currentSessionData.RecordCollectedStars, (int)lastSessionsData.RecordCollectedStars);

    SafeInt? IStatsGetter.RecordEarnedScore => Mathf.Max((int)currentSessionData.RecordEarnedScore, (int)lastSessionsData.RecordEarnedScore);

    SafeInt? IStatsGetter.RecordScoreMultiplierValue => Mathf.Max((int)currentSessionData.RecordScoreMultiplierValue, (int)lastSessionsData.RecordScoreMultiplierValue);

    SafeInt? IStatsGetter.RecordLifeTime => Mathf.Max((int)currentSessionData.RecordLifeTime, (int)lastSessionsData.RecordLifeTime);

    SafeInt? IStatsGetter.TotalLifeTime => currentSessionData.TotalLifeTime + lastSessionsData.TotalLifeTime;
}
