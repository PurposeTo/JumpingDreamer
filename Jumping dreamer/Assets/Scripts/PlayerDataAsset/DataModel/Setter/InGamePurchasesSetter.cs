public class InGamePurchasesSetter : IInGamePurchasesSetter
{
    private readonly InGamePurchases1 inGamePurchases;


    public InGamePurchasesSetter(InGamePurchases1 inGamePurchases)
    {
        this.inGamePurchases = inGamePurchases;
    }


    void IInGamePurchasesSetter.SaveTotalStarsData(SafeInt starsAmount)
    {
        inGamePurchases.TotalStars += starsAmount;
        inGamePurchases.EstimatedCostInStars += starsAmount;
    }

}
