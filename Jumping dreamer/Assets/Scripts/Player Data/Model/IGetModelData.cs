public interface IGetModelData
{
    string Id { get; }
    IGetStatsData StatsData { get; }
    IGetInGamePurchasesData InGamePurchasesData { get; }
}
