public interface IDataSetter
{
    IStatsSetter Stats { get; }
    IInGamePurchasesSetter InGamePurchases { get; }
}
