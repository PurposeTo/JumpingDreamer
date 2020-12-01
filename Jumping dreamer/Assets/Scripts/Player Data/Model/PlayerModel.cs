using System;

public class PlayerModel : ISetDataModel
{
    public const string FileName = "GameData";
    public const string FileExtension = ".json";
    public static string FileNameWithExtension => FileName + FileExtension;

    ISetStatsData ISetDataModel.PlayerStats => stats;
    ISetPlayerInGamePurchases ISetDataModel.PlayerInGamePurchases => inGamePurchases;

    private PlayerModelData data;

    private PlayerStats stats = new PlayerStats();
    private PlayerInGamePurchases inGamePurchases = new PlayerInGamePurchases();


    public PlayerStats GetPlayerStats() => stats;
    public PlayerModelData GetData() => data;
    public void SetData(PlayerModelData modelData) => data = modelData;


    public void SetDataWithDefaultValues()
    {
        stats.SetDataWithDefaultValues();
        inGamePurchases.SetDataWithDefaultValues();

        data = new PlayerModelData
        {
            Id = new Random().Next().ToString(),
            StatsData = stats.GetDataContainer(),
            InGamePurchasesData = inGamePurchases.GetData()
        };
    }


    public bool HasDataNullValues()
    {
        if (string.IsNullOrEmpty(data.Id) || data.StatsData == null || data.InGamePurchasesData == null)
        {
            return false;
        }

        return stats.HasDataNullValues() ||
            inGamePurchases.HasDataNullValues();
    }


    public static PlayerModelData CombineData(PlayerModelData cloudData, PlayerModelData localModel)
    {
        if (cloudData is null) throw new ArgumentNullException(nameof(cloudData));
        if (localModel is null) throw new ArgumentNullException(nameof(localModel));

        PlayerModelData mixedModelData = new PlayerModelData
        {
            StatsData = PlayerStats.CombineData(cloudData.StatsData, localModel.StatsData),
            InGamePurchasesData = PlayerInGamePurchases.CombineData(cloudData.InGamePurchasesData, localModel.InGamePurchasesData)
        };

        return mixedModelData;
    }
}