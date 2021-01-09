using System.Collections.Generic;
using Newtonsoft.Json;

public class InGamePurchasesData1 : IInGamePurchasesGetter
{
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? TotalStars { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? EstimatedCostInStars { get; set; } // Не может уменьшаться!


    public static InGamePurchasesData1 CreatePurchasesWithDefaultValues()
    {
        return new InGamePurchasesData1
        {
            TotalStars = default,
            EstimatedCostInStars = default
        };
    }



    public override string ToString()
    {
        return $"{{\n{TotalStars},\n{EstimatedCostInStars}\n}}";
    }


    public override bool Equals(object obj)
    {
        return obj is PlayerInGamePurchasesData purchases &&
               EqualityComparer<SafeInt?>.Default.Equals(TotalStars, purchases.TotalStars) &&
               EqualityComparer<SafeInt?>.Default.Equals(EstimatedCostInStars, purchases.EstimatedCostInStars);
    }


    public override int GetHashCode()
    {
        int hashCode = -1596784190;
        hashCode = hashCode * -1521134295 + TotalStars.GetHashCode();
        hashCode = hashCode * -1521134295 + EstimatedCostInStars.GetHashCode();

        return hashCode;
    }
}
