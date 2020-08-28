using System;

public class PlayerDataModel
{
    public const string FileName = "GameData.json";

    public string Id { get; set; }
    public PlayerStatsData PlayerStats { get; set; }
    public PlayerInGamePurchases PlayerInGamePurchases { get; set; }


    public static PlayerDataModel CreateModelWithDefaultValues()
    {
        return new PlayerDataModel
        {
            Id = new Random().Next().ToString(),
            PlayerStats = PlayerStatsData.CreateStatsWithDefaultValues(),
            PlayerInGamePurchases = PlayerInGamePurchases.CreateInGamePurchasesWithDefaultValues()
        };
    }


    public bool IsModelHasNullValues()
    {
        if (string.IsNullOrEmpty(Id) || PlayerStats == null || PlayerInGamePurchases == null)
        {
            return false;
        }

        return PlayerStats.IsStatsHaveNullValues() || PlayerInGamePurchases.IsInGamePurchasesHaveNullValues();
    }
}