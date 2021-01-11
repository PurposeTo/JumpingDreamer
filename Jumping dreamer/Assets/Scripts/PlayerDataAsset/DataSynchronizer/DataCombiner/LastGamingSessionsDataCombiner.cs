using UnityEngine;

public class LastGamingSessionsDataCombiner
{
    public DataCombiner dataCombiner = new DataCombiner();


    public PlayerGameData Combine(PlayerGameData data1, PlayerGameData data2)
    {
        // Смешение данных различных хранилищ происходит <=>, когда их id совпадает.
        if (!data1.Id.Equals(data2.Id)) Debug.LogError("Произошло смешение данных с различным id.");

        PlayerGameData data = dataCombiner.Combine(data1, data2);
        data.Id = data1.Id;

        return data;
    }


    private PlayerStatsData CombineStats(PlayerStatsData stats1, PlayerStatsData stats2)
    {
        PlayerStatsData combinedStats = new PlayerStatsData();
        dataCombiner.CombineRecordStats(stats1, stats2, ref combinedStats);
        dataCombiner.CombineTotalStatsForLastGamesData(stats1, stats2, ref combinedStats);

        return combinedStats;
    }


    private InGamePurchasesData CombinePurchases(InGamePurchasesData purchases1, InGamePurchasesData purchases2)
    {
        return purchases1.EstimatedCostInStars > purchases2.EstimatedCostInStars ? purchases1 : purchases2;
    }
}
