using Newtonsoft.Json;

public class PlayerInGamePurchases
{
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? TotalCollectedStars { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? CostValueIndexStars { get; set; } // Не может уменьшаться!


    public static PlayerInGamePurchases CreateInGamePurchasesWithDefaultValues()
    {
        return new PlayerInGamePurchases
        {
            TotalCollectedStars = default(int),
            CostValueIndexStars = default(int)
        };
    }


    public void SaveTotalStarsData(SafeInt starsAmount)
    {
        TotalCollectedStars += starsAmount;
        CostValueIndexStars += starsAmount;
    }


    public bool IsInGamePurchasesHaveNullValues()
    {
        return !TotalCollectedStars.HasValue ||
          !CostValueIndexStars.HasValue;
    }
}
