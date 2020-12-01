using System.Collections.Generic;

public class PlayerModelData : IGetModelData
{
    IGetStatsData IGetModelData.StatsData => StatsData;
    IGetPlayerInGamePurchasesData IGetModelData.InGamePurchasesData => InGamePurchasesData;

    public string Id { get; set; }
    public PlayerStatsData StatsData { get; set; }
    public PlayerInGamePurchasesData InGamePurchasesData { get; set; }


    public override bool Equals(object obj)
    {
        return obj is PlayerModelData model &&
               Id == model.Id &&
               EqualityComparer<PlayerStatsData>.Default.Equals(StatsData, model.StatsData) &&
               EqualityComparer<PlayerInGamePurchasesData>.Default.Equals(InGamePurchasesData, model.InGamePurchasesData);
    }


    public override int GetHashCode()
    {
        int hashCode = -125219266;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
        hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStatsData>.Default.GetHashCode(StatsData);
        hashCode = hashCode * -1521134295 + EqualityComparer<PlayerInGamePurchasesData>.Default.GetHashCode(InGamePurchasesData);

        return hashCode;
    }
}
