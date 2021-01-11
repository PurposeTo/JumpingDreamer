using System.Collections.Generic;
using Newtonsoft.Json;

public class InGamePurchasesData : IInGamePurchasesGetter
{
    public InGamePurchasesData() { }

    public InGamePurchasesData(IInGamePurchasesGetter dataGetter)
    {
        if (dataGetter == null) throw new System.ArgumentNullException(nameof(dataGetter));

        EstimatedCostInStars = dataGetter.EstimatedCostInStars;
        TotalStars = dataGetter.TotalStars;
    }


    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? EstimatedCostInStars { get; set; } // Не может уменьшаться!
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? TotalStars { get; set; }


    public static InGamePurchasesData CreatePurchasesWithDefaultValues()
    {
        return new InGamePurchasesData
        {
            TotalStars = default(int),
            EstimatedCostInStars = default(int)
        };
    }



    public override string ToString()
    {
        return $"{{\n{TotalStars},\n{EstimatedCostInStars}\n}}";
    }


    public override bool Equals(object obj)
    {
        return obj is InGamePurchasesData purchases &&
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
