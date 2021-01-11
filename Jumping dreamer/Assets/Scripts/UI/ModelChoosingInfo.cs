public class ModelChoosingInfo
{
    private readonly IDataGetter localData;
    private readonly IDataGetter cloudData;

    public ModelChoosingInfo(IDataGetter localData, IDataGetter cloudData)
    {
        this.localData = localData ?? throw new System.ArgumentNullException(nameof(localData));
        this.cloudData = cloudData ?? throw new System.ArgumentNullException(nameof(cloudData));
    }


    public IDataGetter SelectedData { get; private set; }
    public bool IsModelSelected { get; private set; } = false;


    public void ChooseLocalModel()
    {
        SelectedData = localData;
        IsModelSelected = true;
    }


    public void ChooseCloudModel()
    {
        SelectedData = cloudData;
        IsModelSelected = true;
    }


    public string GetCloudData()
    {
        return GetData();
    }


    public string GetLocalData()
    {
        return GetData();
    }


    private string GetData(/**/)
    {
        return default;
    }
}
