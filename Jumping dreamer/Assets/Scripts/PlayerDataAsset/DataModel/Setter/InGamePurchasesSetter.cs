public class InGamePurchasesSetter : IInGamePurchasesSetter
{
    private readonly InGamePurchasesData1 inGamePurchases;


    public InGamePurchasesSetter(InGamePurchasesData1 inGamePurchases)
    {
        this.inGamePurchases = inGamePurchases;
    }


    void IInGamePurchasesSetter.AddTotalStars(SafeInt starsAmount)
    {
        inGamePurchases.TotalStars += starsAmount;
        inGamePurchases.EstimatedCostInStars += starsAmount;
    }

}
