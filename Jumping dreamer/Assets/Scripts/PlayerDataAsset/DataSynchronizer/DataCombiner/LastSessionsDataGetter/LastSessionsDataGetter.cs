using System;
using UnityEngine;


/// <summary>
/// Объединяет данные за текущую игровую сессию и прошлые игровые сессии
/// </summary>
public class LastSessionsDataGetter : IDataGetter
{
    public LastSessionsDataGetter(IDataGetter currentData, IDataGetter receivedData)
    {
        //currentData могут быть с дефолтными значениями (т.е. пустые)

        Id = receivedData.Id;
        Stats = new LastSessionsStatsGetter(currentData.Stats, receivedData.Stats);
        InGamePurchases = new LastSessionsInGamePurchasesGetter(currentData.InGamePurchases, receivedData.InGamePurchases);
    }


    public string Id { get; }
    public IStatsGetter Stats { get; }
    public IInGamePurchasesGetter InGamePurchases { get; }
}
