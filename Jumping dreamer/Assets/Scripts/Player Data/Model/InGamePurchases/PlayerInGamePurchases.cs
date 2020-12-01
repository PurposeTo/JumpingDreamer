public class PlayerInGamePurchases : ISetPlayerInGamePurchases
{
    private PlayerInGamePurchasesData data;


    public PlayerInGamePurchasesData GetData() => data;
    public void SetData(PlayerInGamePurchasesData data) => this.data = data;


    public void SetDataWithDefaultValues()
    {
        data = new PlayerInGamePurchasesData
        {
            TotalStars = default(int),
            EstimatedCostInStars = default(int)
        };
    }


    void ISetPlayerInGamePurchases.SaveTotalStarsData(SafeInt starsAmount)
    {
        data.TotalStars += starsAmount;
        data.EstimatedCostInStars += starsAmount;
    }


    public bool HasDataNullValues()
    {
        return !data.TotalStars.HasValue ||
          !data.EstimatedCostInStars.HasValue;
    }


    public static PlayerInGamePurchasesData CombineData(PlayerInGamePurchasesData cloudData, PlayerInGamePurchasesData localData)
    {
        if (cloudData is null) throw new System.ArgumentNullException(nameof(cloudData));
        if (localData is null) throw new System.ArgumentNullException(nameof(localData));

        return cloudData.EstimatedCostInStars > localData.EstimatedCostInStars ? cloudData : localData;
    }
}
