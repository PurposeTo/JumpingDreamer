public interface IInGamePurchasesGetter
{
    SafeInt? TotalStars { get; }
    SafeInt? EstimatedCostInStars { get; } // Не может уменьшаться!
}
