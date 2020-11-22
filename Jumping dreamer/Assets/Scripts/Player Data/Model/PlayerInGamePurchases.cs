using System.Collections.Generic;
using Newtonsoft.Json;

public class PlayerInGamePurchases
{
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? TotalStars { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? EstimatedCostInStars { get; set; } // Не может уменьшаться!


    public static PlayerInGamePurchases CreateInGamePurchasesWithDefaultValues()
    {
        return new PlayerInGamePurchases
        {
            TotalStars = default(int),
            EstimatedCostInStars = default(int)
        };
    }


    public void SaveTotalStarsData(SafeInt starsAmount)
    {
        TotalStars += starsAmount;
        EstimatedCostInStars += starsAmount;
    }


    public bool IsInGamePurchasesHaveNullValues()
    {
        return !TotalStars.HasValue ||
          !EstimatedCostInStars.HasValue;
    }


    public static PlayerInGamePurchases CombinePlayerInGamePurchases(PlayerInGamePurchases cloudPlayerInGamePurchasesData, PlayerInGamePurchases localPlayerInGamePurchasesData)
    {
        if (cloudPlayerInGamePurchasesData is null) throw new System.ArgumentNullException(nameof(cloudPlayerInGamePurchasesData));
        if (localPlayerInGamePurchasesData is null) throw new System.ArgumentNullException(nameof(localPlayerInGamePurchasesData));

        return cloudPlayerInGamePurchasesData.EstimatedCostInStars > localPlayerInGamePurchasesData.EstimatedCostInStars ? cloudPlayerInGamePurchasesData : localPlayerInGamePurchasesData;
    }


    #region Моя реализация Equals
    //public override bool Equals(object obj)
    //{
    //    return obj is PlayerInGamePurchases && base.Equals(obj);
    //}
    #endregion


    // TODO: Работает?
    public override bool Equals(object obj)
    {
        return obj is PlayerInGamePurchases purchases &&
               EqualityComparer<SafeInt?>.Default.Equals(TotalStars, purchases.TotalStars) &&
               EqualityComparer<SafeInt?>.Default.Equals(EstimatedCostInStars, purchases.EstimatedCostInStars);
    }


    // TODO: Работает?
    public override int GetHashCode()
    {
        int hashCode = -1596784190;
        hashCode = hashCode * -1521134295 + TotalStars.GetHashCode();
        hashCode = hashCode * -1521134295 + EstimatedCostInStars.GetHashCode();

        return hashCode;
    }
}
