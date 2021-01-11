using System;
using UnityEngine;

public class UsedInGamePurchasesGetter : IInGamePurchasesGetter
{
    private readonly IInGamePurchasesGetter currentSessionData;
    private readonly IInGamePurchasesGetter lastSessionsData;

    public UsedInGamePurchasesGetter(IInGamePurchasesGetter currentSessionData, IInGamePurchasesGetter lastSessionsData)
    {
        this.currentSessionData = currentSessionData ?? throw new ArgumentNullException(nameof(currentSessionData));
        this.lastSessionsData = lastSessionsData;
    }

    SafeInt? IInGamePurchasesGetter.TotalStars =>
                lastSessionsData == null || lastSessionsData.TotalStars == null
                ? currentSessionData.TotalStars
                : (SafeInt)(currentSessionData.TotalStars + lastSessionsData.TotalStars);

    SafeInt? IInGamePurchasesGetter.EstimatedCostInStars =>
                        lastSessionsData == null || lastSessionsData.EstimatedCostInStars == null
                ? currentSessionData.EstimatedCostInStars
                : (SafeInt)(currentSessionData.EstimatedCostInStars + lastSessionsData.EstimatedCostInStars);
}
