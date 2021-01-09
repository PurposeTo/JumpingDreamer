using System;

public class DataModel : IDataInteraction, IDataChangingNotifier, IModelInteraction
{
    public const string FileName = "GameData";
    public const string FileExtension = ".json";
    public static string FileNameWithExtension => FileName + FileExtension;

    private readonly DataSetter dataSetter;


    public DataModel()
    {
        dataSetter = new DataSetter(data);
    }


    IDataGetter IDataInteraction.Getter => data;
    IDataSetter IDataInteraction.Setter => dataSetter;
    IDataChangingNotifier IDataInteraction.Notifier => this;


    public event Action OnDataReset;
    IStatsChangingNotifier IDataChangingNotifier.StatsChangingNotifier => dataSetter.StatsChangingNotifier;

    private Data data = Data.CreateDataWithDefaultValues();


    Data IModelInteraction.GetData()
    {
        return data;
    }


    void IModelInteraction.SetData(Data data)
    {
        this.data = data;
    }
}
