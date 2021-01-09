public interface IInGamePurchasesGetter
{
    public SafeInt? TotalStars { get; }
    public SafeInt? EstimatedCostInStars { get; } // Не может уменьшаться!
}
