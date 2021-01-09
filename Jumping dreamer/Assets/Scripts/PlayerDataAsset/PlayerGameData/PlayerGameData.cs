using System;
using System.Collections.Generic;

[Serializable]
public class PlayerGameData : IDataGetter
{
    IStatsGetter IDataGetter.Stats => Stats;
    IInGamePurchasesGetter IDataGetter.InGamePurchases => InGamePurchases;

    public string Id { get; set; }
    public PlayerStatsData1 Stats { get; set; }
    public InGamePurchasesData1 InGamePurchases { get; set; }


    public static PlayerGameData CreateDataWithDefaultValues()
    {
        return new PlayerGameData
        {
            Id = new Random().Next().ToString(),
            Stats = PlayerStatsData1.CreateStatsWithDefaultValues(),
            InGamePurchases = InGamePurchasesData1.CreatePurchasesWithDefaultValues()
        };
    }


    public override string ToString()
    {
        return $"{{\n{Id},\n{Stats},\n{InGamePurchases}\n}}";
    }


    public override bool Equals(object obj)
    {
        return obj is PlayerGameData data &&
               Id == data.Id &&
               EqualityComparer<PlayerStatsData1>.Default.Equals(Stats, data.Stats) &&
               EqualityComparer<InGamePurchasesData1>.Default.Equals(InGamePurchases, data.InGamePurchases);
    }


    public override int GetHashCode()
    {
        int hashCode = 1777818016;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
        hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStatsData1>.Default.GetHashCode(Stats);
        hashCode = hashCode * -1521134295 + EqualityComparer<InGamePurchasesData1>.Default.GetHashCode(InGamePurchases);

        return hashCode;
    }
}
