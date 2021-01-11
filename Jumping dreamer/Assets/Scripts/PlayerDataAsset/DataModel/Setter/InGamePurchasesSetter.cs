public class InGamePurchasesSetter : IInGamePurchasesSetter
{
    private readonly InGamePurchasesData inGamePurchases;


    public InGamePurchasesSetter(InGamePurchasesData inGamePurchases)
    {
        this.inGamePurchases = inGamePurchases;
    }


    void IInGamePurchasesSetter.AddTotalStars(SafeInt starsAmount)
    {
        inGamePurchases.TotalStars += starsAmount;
        inGamePurchases.EstimatedCostInStars += starsAmount;
    }

}
