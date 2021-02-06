using System;
using UnityEngine;


/// <summary>
/// Объединяет данные за текущую игровую сессию и прошлые игровые сессии
/// </summary>
public class UsedDataGetter : IDataGetter
{
    private readonly IStatsGetter usedStatsGetter;
    private readonly IInGamePurchasesGetter usedInGamePurchasesGetter;

    public UsedDataGetter(IDataGetter currentSessionData, IDataGetter lastSessionsData)
    {
        if (currentSessionData == null) throw new ArgumentNullException(nameof(currentSessionData));
        if (lastSessionsData == null) throw new ArgumentNullException(nameof(lastSessionsData));

        Id = currentSessionData.Id;
        usedStatsGetter = new UsedStatsGetter(currentSessionData.Stats, lastSessionsData.Stats);
        usedInGamePurchasesGetter = new UsedInGamePurchasesGetter(currentSessionData.InGamePurchases, lastSessionsData.InGamePurchases);
    }


    public string Id { get; }

    IStatsGetter IDataGetter.Stats => usedStatsGetter;

    IInGamePurchasesGetter IDataGetter.InGamePurchases => usedInGamePurchasesGetter;
}
