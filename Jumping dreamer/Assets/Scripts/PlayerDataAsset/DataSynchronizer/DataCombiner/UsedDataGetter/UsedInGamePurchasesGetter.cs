using System;
using UnityEngine;

public class UsedInGamePurchasesGetter : IInGamePurchasesGetter
{
    private readonly IInGamePurchasesGetter currentSessionData;
    private readonly IInGamePurchasesGetter lastSessionsData;

    public UsedInGamePurchasesGetter(IInGamePurchasesGetter currentSessionData, IInGamePurchasesGetter lastSessionsData)
    {
        this.currentSessionData = currentSessionData ?? throw new ArgumentNullException(nameof(currentSessionData));
        this.lastSessionsData = lastSessionsData ?? throw new ArgumentNullException(nameof(lastSessionsData));
    }

    SafeInt? IInGamePurchasesGetter.TotalStars => currentSessionData.TotalStars + lastSessionsData.TotalStars;

    SafeInt? IInGamePurchasesGetter.EstimatedCostInStars => currentSessionData.EstimatedCostInStars + lastSessionsData.EstimatedCostInStars;
}
