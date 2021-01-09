public interface IDataGetter
{
    string Id { get; }
    IStatsGetter Stats { get; }
    IInGamePurchasesGetter InGamePurchases { get; }
}
