using UnityEngine;

public class ModelChoosingWindow : MonoBehaviour
{
    public PlayerDataModel SelectedDataModel;
    public PlayerDataModelType SelectedDataModelType;

    private PlayerDataModel localModel;
    private PlayerDataModel cloudModel;


    public void Constructor(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        this.localModel = localModel;
        this.cloudModel = cloudModel;
    }


    public void ChooseLocalModelData()
    {
        SelectedDataModel = localModel;
        SelectedDataModelType = PlayerDataModelType.LocalModel;
    }


    public void ChooseCloudModelData()
    {
        SelectedDataModel = cloudModel;
        SelectedDataModelType = PlayerDataModelType.CloudModel;
    }


    public void CloseWindow() => Destroy(gameObject);
}
