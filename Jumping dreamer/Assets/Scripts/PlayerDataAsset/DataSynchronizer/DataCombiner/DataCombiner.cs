using UnityEngine;
using System;

public class DataCombiner
{
    public PlayerGameData Combine(PlayerGameData data1, PlayerGameData data2)
    {
        if (data1 == null) throw new ArgumentNullException(nameof(data1));
        if (data2 == null) throw new ArgumentNullException(nameof(data2));

        throw new NotImplementedException();
        //return new PlayerGameData
        //{
        //    Stats = CombineRecordStats(data1.Stats, data1.Stats),
        //    InGamePurchases = CombinePurchases(data1.InGamePurchases, data2.InGamePurchases)
        //};
    }


    public void CombineRecordStats(PlayerStatsData stats1, PlayerStatsData stats2, ref PlayerStatsData combinedRecordStats)
    {
        combinedRecordStats.RecordCollectedStars = Mathf.Max((int)stats1.RecordCollectedStars, (int)stats2.RecordCollectedStars);
        combinedRecordStats.RecordEarnedScore = Mathf.Max((int)stats1.RecordEarnedScore, (int)stats2.RecordEarnedScore);
        combinedRecordStats.RecordLifeTime = Mathf.Max((int)stats1.RecordLifeTime, (int)stats2.RecordLifeTime);
        combinedRecordStats.RecordScoreMultiplierValue = Mathf.Max((int)stats1.RecordScoreMultiplierValue, (int)stats2.RecordScoreMultiplierValue);
    }


    public void CombineTotalStatsForLastGamesData(PlayerStatsData stats1, PlayerStatsData stats2, ref PlayerStatsData combinedStats)
    {
        combinedStats.TotalLifeTime = Mathf.Max((int)stats1.TotalLifeTime, (int)stats2.TotalLifeTime);
    }


    public void CombineTotalStarsForCurrentGameData(PlayerStatsData stats1, PlayerStatsData stats2, ref PlayerStatsData combinedStats)
    {
        combinedStats.TotalLifeTime = stats1.TotalLifeTime + stats2.TotalLifeTime;
    }


    private InGamePurchasesData CombinePurchasesForLastGamesData(InGamePurchasesData purchases1, InGamePurchasesData purchases2)
    {
        throw new NotImplementedException();
    }


    private InGamePurchasesData CombinePurchasesForCurrentGameData(InGamePurchasesData purchases1, InGamePurchasesData purchases2)
    {
        throw new NotImplementedException();
    }
}
