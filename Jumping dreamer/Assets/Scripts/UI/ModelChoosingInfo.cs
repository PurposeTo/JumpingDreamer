public class ModelChoosingInfo
{
    public PlayerModelDataType SelectedModelDataType;
    public bool IsModelSelected { get; private set; } = false;


    public void ChooseLocalModel()
    {
        SelectedModelDataType = PlayerModelDataType.LocalModel;
        IsModelSelected = true;
    }


    public void ChooseCloudModel()
    {
        SelectedModelDataType = PlayerModelDataType.CloudModel;
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
