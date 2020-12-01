using UnityEngine;

public class ModelChoosingWindow : MonoBehaviour
{
    public IGetStatsData SelectedDataModel;
    public PlayerModelDataType SelectedModelDataType;

    private IGetStatsData localModel;
    private IGetStatsData cloudModel;


    public void Constructor(IGetStatsData localModelData, IGetStatsData cloudModelData)
    {
        localModel = localModelData;
        cloudModel = cloudModelData;
    }


    public void ChooseLocalModelData()
    {
        SelectedDataModel = localModel;
        SelectedModelDataType = PlayerModelDataType.LocalModel;
    }


    public void ChooseCloudModelData()
    {
        SelectedDataModel = cloudModel;
        SelectedModelDataType = PlayerModelDataType.CloudModel;
    }


    public void CloseWindow() => Destroy(gameObject);
}
