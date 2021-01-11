using System;
using UnityEngine;

public class DataModel : IDataInteraction, IDataChangingNotifier, IModelInteraction
{
    public const string FileName = "GameData";
    public const string FileExtension = ".json";
    public static string FileNameWithExtension => FileName + FileExtension;

    private readonly DataSetter dataSetter;
    private PlayerGameData data;


    public DataModel()
    {
        data = PlayerGameData.CreateDataWithDefaultValues();
        dataSetter = new DataSetter(data);
    }


    IDataGetter IDataInteraction.Getter => data;
    IDataSetter IDataInteraction.Setter => dataSetter;
    IDataChangingNotifier IDataInteraction.Notifier => this;

    public event Action OnDataReset;
    IStatsChangingNotifier IDataChangingNotifier.StatsChangingNotifier => dataSetter.StatsChangingNotifier;


    PlayerGameData IModelInteraction.GetData() => data;


    void IModelInteraction.SetData(PlayerGameData data)
    {
        this.data = data;
    }
}
