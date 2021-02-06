using System;
using UnityEngine;

public class LastSessionsStatsGetter : IStatsGetter
{
    public LastSessionsStatsGetter(IStatsGetter currentData, IStatsGetter receivedData)
    {
        //currentData могут быть с дефолтными значениями (т.е. пустые)

        RecordCollectedStars = Mathf.Max((int)currentData.RecordCollectedStars, (int)receivedData.RecordCollectedStars);
        RecordEarnedScore = Mathf.Max((int)currentData.RecordEarnedScore, (int)receivedData.RecordEarnedScore);
        RecordScoreMultiplierValue = Mathf.Max((int)currentData.RecordScoreMultiplierValue, (int)receivedData.RecordScoreMultiplierValue);
        RecordLifeTime = Mathf.Max((int)currentData.RecordLifeTime, (int)receivedData.RecordLifeTime);
        TotalLifeTime = Mathf.Max((int)currentData.TotalLifeTime, (int)receivedData.TotalLifeTime);
    }


    public SafeInt? RecordCollectedStars { get; }
    public SafeInt? RecordEarnedScore { get; }
    public SafeInt? RecordScoreMultiplierValue { get; }
    public SafeInt? RecordLifeTime { get; }
    public SafeInt? TotalLifeTime { get; }
}
