using UnityEngine;

public class CurrentGameSessionsDataCombiner
{
    private DataCombiner dataCombiner = new DataCombiner();

    /// <summary>
    /// Операция "Смешение" данных модели.
    /// </summary>
    /// <param name="currentGameSessionData">Данные за текущую игровую сессию</param>
    /// <param name="lastGameSessionData">Данные за прошлые игровые сессии</param>
    /// <returns></returns>
    public PlayerGameData Combine(PlayerGameData currentGameSessionData, PlayerGameData lastGameSessionData)
    {
        PlayerGameData gameData = dataCombiner.Combine(currentGameSessionData, lastGameSessionData);
        gameData.Id = lastGameSessionData.Id;

        return gameData;
    }


    private PlayerStatsData1 CombineStats(PlayerStatsData1 stats1, PlayerStatsData1 stats2)
    {
        PlayerStatsData1 combinedStats = new PlayerStatsData1();
        dataCombiner.CombineRecordStats(stats1, stats2, ref combinedStats);
        dataCombiner.CombineTotalStarsForCurrentGameData(stats1, stats2, ref combinedStats);

        return combinedStats;
    }


    private InGamePurchasesData1 CombinePurchases(InGamePurchasesData1 purchases1, InGamePurchasesData1 purchases2)
    {
        if (!purchases1.EstimatedCostInStars.Equals(purchases2.EstimatedCostInStars))
        {
            Debug.LogError("Произошло смешение данных с различной оценочной стоимостью звезд.");
        }

        return new InGamePurchasesData1
        {
            EstimatedCostInStars = purchases1.EstimatedCostInStars,
            TotalStars = purchases1.TotalStars + purchases2.TotalStars
        };
    }
}
