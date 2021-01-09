using UnityEngine;

public class DataCombiner
{
    public PlayerGameData Combine(PlayerGameData data1, PlayerGameData data2)
    {
        if (data1 == null) throw new System.ArgumentNullException(nameof(data1));
        if (data2 == null) throw new System.ArgumentNullException(nameof(data2));
        
        return new PlayerGameData
        {
            Id = data1.Id, // Поля id обеих моделей одинаковы в случае смешения этих моделей
            Stats = CombineStats(data1.Stats, data1.Stats),
            InGamePurchases = CombinePurchases(data1.InGamePurchases, data2.InGamePurchases)
        };
    }


    private PlayerStatsData1 CombineStats(PlayerStatsData1 stats1, PlayerStatsData1 stats2)
    {
        return new PlayerStatsData1
        {
            RecordCollectedStars = Mathf.Max((int)stats1.RecordCollectedStars, (int)stats2.RecordCollectedStars),
            RecordEarnedScore = Mathf.Max((int)stats1.RecordEarnedScore, (int)stats2.RecordEarnedScore),
            RecordLifeTime = Mathf.Max((int)stats1.RecordLifeTime, (int)stats2.RecordLifeTime),
            RecordScoreMultiplierValue = Mathf.Max((int)stats1.RecordScoreMultiplierValue, (int)stats2.RecordScoreMultiplierValue),
            TotalLifeTime = Mathf.Max((int)stats1.TotalLifeTime, (int)stats2.TotalLifeTime)
        };
    }


    private InGamePurchasesData1 CombinePurchases(InGamePurchasesData1 purchases1, InGamePurchasesData1 purchases2)
    {
        return purchases1.EstimatedCostInStars > purchases2.EstimatedCostInStars ? purchases1 : purchases2;
    }
}
