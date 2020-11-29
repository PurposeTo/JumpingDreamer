public interface IGetDataModel
{
    public string Id { get; }
    public IGetStatsData PlayerStats { get; }
    public IGetInGamePurchases PlayerInGamePurchases { get; }
}
