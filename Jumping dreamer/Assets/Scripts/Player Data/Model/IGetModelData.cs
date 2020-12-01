public interface IGetModelData
{
    string Id { get; }
    IGetStatsData StatsData { get; }
    IGetPlayerInGamePurchasesData InGamePurchasesData { get; }
}
