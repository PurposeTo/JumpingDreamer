using Newtonsoft.Json;

public class PlayerInGamePurchases
{
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? TotalCollectedStars { get; set; }


    public static PlayerInGamePurchases CreateInGamePurchasesWithDefaultValues()
    {
        return new PlayerInGamePurchases
        {
            TotalCollectedStars = default(int)
        };
    }


    public void SaveTotalStarsData(SafeInt starsAmount)
    {
        PlayerDataStorageSafe.Instance.PlayerDataModel.PlayerInGamePurchases.TotalCollectedStars += starsAmount;
    }


    public bool IsInGamePurchasesHaveNullValues()
    {
        return !TotalCollectedStars.HasValue;
    }
}
