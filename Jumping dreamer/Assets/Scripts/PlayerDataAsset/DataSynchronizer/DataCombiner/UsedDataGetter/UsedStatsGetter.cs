using System;
using UnityEngine;

public class UsedStatsGetter : IStatsGetter
{
    private readonly IStatsGetter currentSessionData;
    private readonly IStatsGetter lastSessionsData;

    public UsedStatsGetter(IStatsGetter currentSessionData, IStatsGetter lastSessionsData)
    {
        this.currentSessionData = currentSessionData ?? throw new ArgumentNullException(nameof(currentSessionData));
        this.lastSessionsData = lastSessionsData;
    }


    SafeInt? IStatsGetter.RecordCollectedStars =>
        lastSessionsData == null || lastSessionsData.RecordCollectedStars == null
                ? currentSessionData.RecordCollectedStars
                : Mathf.Max((int)currentSessionData.RecordCollectedStars, (int)lastSessionsData.RecordCollectedStars);

    SafeInt? IStatsGetter.RecordEarnedScore =>
        lastSessionsData == null || lastSessionsData.RecordEarnedScore == null
                ? currentSessionData.RecordEarnedScore
                : Mathf.Max((int)currentSessionData.RecordEarnedScore, (int)lastSessionsData.RecordEarnedScore);

    SafeInt? IStatsGetter.RecordScoreMultiplierValue =>
        lastSessionsData == null || lastSessionsData.RecordScoreMultiplierValue == null
                ? currentSessionData.RecordScoreMultiplierValue
                : Mathf.Max((int)currentSessionData.RecordScoreMultiplierValue, (int)lastSessionsData.RecordScoreMultiplierValue);

    SafeInt? IStatsGetter.RecordLifeTime =>
        lastSessionsData == null || lastSessionsData.RecordLifeTime == null
                ? currentSessionData.RecordLifeTime
                : Mathf.Max((int)currentSessionData.RecordLifeTime, (int)lastSessionsData.RecordLifeTime);

    SafeInt? IStatsGetter.TotalLifeTime =>
        lastSessionsData == null || lastSessionsData.TotalLifeTime == null
                ? currentSessionData.TotalLifeTime
                : (SafeInt)(currentSessionData.TotalLifeTime + lastSessionsData.TotalLifeTime);
}
