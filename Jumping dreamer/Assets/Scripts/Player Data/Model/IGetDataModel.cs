public interface IGetDataModel
{
    string Id { get; }
    IGetStatsData PlayerStats { get; }
    IGetInGamePurchases PlayerInGamePurchases { get; }
}
