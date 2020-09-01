using Newtonsoft.Json;

public class PlayerInGamePurchases
{
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? TotalStars { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? EstimatedCostInStars { get; set; } // Не может уменьшаться!


    public static PlayerInGamePurchases CreateInGamePurchasesWithDefaultValues()
    {
        return new PlayerInGamePurchases
        {
            TotalStars = default(int),
            EstimatedCostInStars = default(int)
        };
    }


    public void SaveTotalStarsData(SafeInt starsAmount)
    {
        TotalStars += starsAmount;
        EstimatedCostInStars += starsAmount;
    }


    public bool IsInGamePurchasesHaveNullValues()
    {
        return !TotalStars.HasValue ||
          !EstimatedCostInStars.HasValue;
    }


    public static PlayerInGamePurchases MixPlayerInGamePurchases(PlayerInGamePurchases cloudPlayerInGamePurchasesData, PlayerInGamePurchases localPlayerInGamePurchasesData)
    {
        if (cloudPlayerInGamePurchasesData == null)
        {
            return localPlayerInGamePurchasesData;
        }

        return cloudPlayerInGamePurchasesData.EstimatedCostInStars > localPlayerInGamePurchasesData.EstimatedCostInStars ? cloudPlayerInGamePurchasesData : localPlayerInGamePurchasesData;
    }
}
