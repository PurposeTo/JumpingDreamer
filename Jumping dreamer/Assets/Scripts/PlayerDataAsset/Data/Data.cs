using System;
using System.Collections.Generic;

[Serializable]
public class Data : IDataGetter
{
    IStatsGetter IDataGetter.Stats => Stats;
    IInGamePurchasesGetter IDataGetter.InGamePurchases => InGamePurchases;

    public string Id { get; set; }
    public PlayerStats1 Stats { get; set; }
    public InGamePurchases1 InGamePurchases { get; set; }


    public static Data CreateDataWithDefaultValues()
    {
        throw new NotImplementedException();
    }


    public override string ToString()
    {
        return Id + Stats.ToString() + InGamePurchases.ToString();
    }


    public override bool Equals(object obj)
    {
        return obj is Data data &&
               Id == data.Id &&
               EqualityComparer<PlayerStats1>.Default.Equals(Stats, data.Stats) &&
               EqualityComparer<InGamePurchases1>.Default.Equals(InGamePurchases, data.InGamePurchases);
    }


    public override int GetHashCode()
    {
        int hashCode = 1777818016;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
        hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStats1>.Default.GetHashCode(Stats);
        hashCode = hashCode * -1521134295 + EqualityComparer<InGamePurchases1>.Default.GetHashCode(InGamePurchases);

        return hashCode;
    }
}
