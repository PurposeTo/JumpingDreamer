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


    public void CombineRecordStats(PlayerStatsData1 stats1, PlayerStatsData1 stats2, ref PlayerStatsData1 combinedRecordStats)
    {
        combinedRecordStats.RecordCollectedStars = Mathf.Max((int)stats1.RecordCollectedStars, (int)stats2.RecordCollectedStars);
        combinedRecordStats.RecordEarnedScore = Mathf.Max((int)stats1.RecordEarnedScore, (int)stats2.RecordEarnedScore);
        combinedRecordStats.RecordLifeTime = Mathf.Max((int)stats1.RecordLifeTime, (int)stats2.RecordLifeTime);
        combinedRecordStats.RecordScoreMultiplierValue = Mathf.Max((int)stats1.RecordScoreMultiplierValue, (int)stats2.RecordScoreMultiplierValue);
    }


    public void CombineTotalStatsForLastGamesData(PlayerStatsData1 stats1, PlayerStatsData1 stats2, ref PlayerStatsData1 combinedStats)
    {
        combinedStats.TotalLifeTime = Mathf.Max((int)stats1.TotalLifeTime, (int)stats2.TotalLifeTime);
    }


    public void CombineTotalStarsForCurrentGameData(PlayerStatsData1 stats1, PlayerStatsData1 stats2, ref PlayerStatsData1 combinedStats)
    {
        combinedStats.TotalLifeTime = stats1.TotalLifeTime + stats2.TotalLifeTime;
    }


    private InGamePurchasesData1 CombinePurchasesForLastGamesData(InGamePurchasesData1 purchases1, InGamePurchasesData1 purchases2)
    {
        throw new NotImplementedException();
    }


    private InGamePurchasesData1 CombinePurchasesForCurrentGameData(InGamePurchasesData1 purchases1, InGamePurchasesData1 purchases2)
    {
        throw new NotImplementedException();
    }
}
