using UnityEngine;

public class MixingModelsWindow : MonoBehaviour
{
    public PlayerDataModel SelectedPlayerDataModel { get; private set; }
    public bool IsSelected { get; private set; }

    private PlayerDataModel localModel;
    private PlayerDataModel cloudModel;


    public void Initialize(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        this.localModel = localModel;
        this.cloudModel = cloudModel;
    }


    public void ChooseLocalModelData()
    {
        SelectedPlayerDataModel = localModel;
        IsSelected = true;
    }


    public void ChooseCloudModelData()
    {
        SelectedPlayerDataModel = cloudModel;
        IsSelected = true;
    }


    public void CloseWindow()
    {
        Destroy(gameObject);
    }
}
