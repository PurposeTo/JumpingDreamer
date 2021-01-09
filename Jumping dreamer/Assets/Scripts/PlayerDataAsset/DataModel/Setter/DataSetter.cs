public class DataSetter : IDataSetter
{
    private readonly StatsSetter statsSetter;
    private readonly InGamePurchasesSetter inGamePurchasesSetter;


    public DataSetter(PlayerGameData data)
    {
        statsSetter = new StatsSetter(data.Stats);
        inGamePurchasesSetter = new InGamePurchasesSetter(data.InGamePurchases);
    }


    IStatsSetter IDataSetter.Stats => statsSetter;
    IInGamePurchasesSetter IDataSetter.InGamePurchases => inGamePurchasesSetter;

    public IStatsChangingNotifier StatsChangingNotifier => statsSetter;
}
