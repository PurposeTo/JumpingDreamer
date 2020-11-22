using System;
using System.Collections.Generic;

public class PlayerDataModel
{
    //public const string FileName = "GameData";
    public const string FileExtension = ".json";
    public static string FileNameWithExtension => FileName + FileExtension;

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


    public static PlayerDataModel CombinePlayerModels(PlayerDataModel cloudPlayerDataModel, PlayerDataModel localPlayerDataModel)
    {
        if (cloudPlayerDataModel is null) throw new ArgumentNullException(nameof(cloudPlayerDataModel));
        if (localPlayerDataModel is null) throw new ArgumentNullException(nameof(localPlayerDataModel));

        PlayerDataModel mixedPlayerDataModel = CreateModelWithDefaultValues();

        mixedPlayerDataModel.PlayerStats = PlayerStatsData.CombinePlayerStats(cloudPlayerDataModel.PlayerStats, localPlayerDataModel.PlayerStats);
        mixedPlayerDataModel.PlayerInGamePurchases = PlayerInGamePurchases.CombinePlayerInGamePurchases(cloudPlayerDataModel.PlayerInGamePurchases, localPlayerDataModel.PlayerInGamePurchases);

        return mixedPlayerDataModel;
    }


    #region Моя реализация Equals
    //public override bool Equals(object obj)
    //{
    //    return obj is PlayerDataModel dataModel &&
    //        Id == dataModel.Id &&
    //        PlayerStats.Equals(dataModel.PlayerStats) &&
    //        PlayerInGamePurchases.Equals(dataModel.PlayerInGamePurchases);
    //}
    #endregion


    // TODO: Работает?
    public override bool Equals(object obj)
    {
        return obj is PlayerDataModel model &&
               Id == model.Id &&
               EqualityComparer<PlayerStatsData>.Default.Equals(PlayerStats, model.PlayerStats) &&
               EqualityComparer<PlayerInGamePurchases>.Default.Equals(PlayerInGamePurchases, model.PlayerInGamePurchases);
    }


    // TODO: Работает?
    public override int GetHashCode()
    {
        int hashCode = -125219266;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
        hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStatsData>.Default.GetHashCode(PlayerStats);
        hashCode = hashCode * -1521134295 + EqualityComparer<PlayerInGamePurchases>.Default.GetHashCode(PlayerInGamePurchases);

        return hashCode;
    }
}