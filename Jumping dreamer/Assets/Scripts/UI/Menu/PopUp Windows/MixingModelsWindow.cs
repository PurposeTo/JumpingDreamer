using UnityEngine;

public class MixingModelsWindow : MonoBehaviour
{
    private PlayerDataModel localModel;
    private PlayerDataModel cloudModel;


    public void Initialize(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        this.localModel = localModel;
        this.cloudModel = cloudModel;
    }


    public void ChooseLocalModelData()
    {
        PlayerDataModelController.Instance.OnDataModelSelected(localModel, PlayerDataModelController.DataModelSelectionStatus.LocalModel);
        CloseWindow();
    }


    public void ChooseCloudModelData()
    {
        PlayerDataModelController.Instance.OnDataModelSelected(cloudModel, PlayerDataModelController.DataModelSelectionStatus.CloudModel);
        CloseWindow();
    }


    private void CloseWindow()
    {
        Destroy(gameObject);
    }
}
