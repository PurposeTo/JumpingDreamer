using UnityEngine;

public class ModelChoosingWindow : MonoBehaviour
{
    private ModelChoosingInfo modelInfo;


    public void Constructor(ModelChoosingInfo modelInfo)
    {
        if (modelInfo == null) throw new System.ArgumentNullException(nameof(modelInfo));

        this.modelInfo = modelInfo;
    }


    public void ChooseLocalModelData()
    {
        modelInfo.ChooseLocalModel();
    }


    public void ChooseCloudModelData()
    {
        modelInfo.ChooseCloudModel();
    }


    public void CloseWindow() => Destroy(gameObject);
}
