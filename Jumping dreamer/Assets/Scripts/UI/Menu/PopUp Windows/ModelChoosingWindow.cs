using System;
using UnityEngine;

public class ModelChoosingWindow : MonoBehaviour
{
    private Action<LoadedPlayerDataModel> selectTheModel;

    public void Constructor(Action<LoadedPlayerDataModel> chooseTheModelCallback)
    {
        selectTheModel = chooseTheModelCallback;
    }


    public void ChooseLocalModelData()
    {
        //PlayerDataModelController.Instance.OnDataModelSelected(localModel, PlayerDataModelController.DataModelSelectionStatus.LocalModel);
        selectTheModel?.Invoke(LoadedPlayerDataModel.LocalModel);
        CloseWindow();
    }


    public void ChooseCloudModelData()
    {
        //PlayerDataModelController.Instance.OnDataModelSelected(cloudModel, PlayerDataModelController.DataModelSelectionStatus.CloudModel);
        selectTheModel?.Invoke(LoadedPlayerDataModel.CloudModel);
        CloseWindow();
    }


    private void CloseWindow() => Destroy(gameObject);
}
