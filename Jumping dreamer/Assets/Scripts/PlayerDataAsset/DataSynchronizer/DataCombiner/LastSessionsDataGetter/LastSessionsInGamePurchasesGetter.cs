using System;
using UnityEngine;

public class LastSessionsInGamePurchasesGetter : IInGamePurchasesGetter
{
    public LastSessionsInGamePurchasesGetter(IInGamePurchasesGetter currentData, IInGamePurchasesGetter receivedData)
    {
        IInGamePurchasesGetter inGamePurchases = currentData.EstimatedCostInStars >= receivedData.EstimatedCostInStars ? currentData : receivedData;

        EstimatedCostInStars = inGamePurchases.EstimatedCostInStars;
        TotalStars = inGamePurchases.TotalStars;
    }


    public SafeInt? TotalStars { get; }
    public SafeInt? EstimatedCostInStars { get; }
}
