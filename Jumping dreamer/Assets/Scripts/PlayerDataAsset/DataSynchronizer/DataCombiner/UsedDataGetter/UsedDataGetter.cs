using System;
using UnityEngine;

public class UsedDataGetter : IDataGetter
{
    private readonly IDataGetter currentSessionData;

    private readonly UsedStatsGetter usedStatsGetter;
    private readonly UsedInGamePurchasesGetter usedInGamePurchasesGetter;

    public UsedDataGetter(IDataGetter currentSessionData, IDataGetter lastSessionsData)
    {
        this.currentSessionData = currentSessionData ?? throw new ArgumentNullException(nameof(currentSessionData));

        IStatsGetter lastSessionsStatsGetter = lastSessionsData == null
            ? null
            : lastSessionsData.Stats;

        IInGamePurchasesGetter lastSessionsInGamePurchasesGetter = lastSessionsData == null
            ? null
            : lastSessionsData.InGamePurchases;

        usedStatsGetter = new UsedStatsGetter(currentSessionData.Stats, lastSessionsStatsGetter);
        usedInGamePurchasesGetter = new UsedInGamePurchasesGetter(currentSessionData.InGamePurchases, lastSessionsInGamePurchasesGetter);
    }


    string IDataGetter.Id => currentSessionData.Id;

    IStatsGetter IDataGetter.Stats => usedStatsGetter;

    IInGamePurchasesGetter IDataGetter.InGamePurchases => usedInGamePurchasesGetter;
}
