using System;
using System.Collections.Generic;

[Serializable]
public class PlayerGameData : IDataGetter
{
    public PlayerGameData() { }

    public PlayerGameData(IDataGetter dataGetter)
    {
        if (dataGetter == null) throw new ArgumentNullException(nameof(dataGetter));

        Id = dataGetter.Id;
        Stats = new PlayerStatsData(dataGetter.Stats);
        InGamePurchases = new InGamePurchasesData(dataGetter.InGamePurchases);
    }


    IStatsGetter IDataGetter.Stats => Stats;
    IInGamePurchasesGetter IDataGetter.InGamePurchases => InGamePurchases;

    public string Id { get; set; }
    public PlayerStatsData Stats { get; set; }
    public InGamePurchasesData InGamePurchases { get; set; }


    public static PlayerGameData CreateDataWithDefaultValues()
    {
        return new PlayerGameData
        {
            Id = new Random().Next().ToString(),
            Stats = PlayerStatsData.CreateStatsWithDefaultValues(),
            InGamePurchases = InGamePurchasesData.CreatePurchasesWithDefaultValues()
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
               EqualityComparer<PlayerStatsData>.Default.Equals(Stats, data.Stats) &&
               EqualityComparer<InGamePurchasesData>.Default.Equals(InGamePurchases, data.InGamePurchases);
    }


    public override int GetHashCode()
    {
        int hashCode = 1777818016;
        hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Id);
        hashCode = hashCode * -1521134295 + EqualityComparer<PlayerStatsData>.Default.GetHashCode(Stats);
        hashCode = hashCode * -1521134295 + EqualityComparer<InGamePurchasesData>.Default.GetHashCode(InGamePurchases);

        return hashCode;
    }
}
