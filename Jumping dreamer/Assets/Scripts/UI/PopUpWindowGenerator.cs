using Desdiene.ObjectPoolerAsset;
using Desdiene.Singleton;
using UnityEngine;

public class PopUpWindowGenerator : SingletonSuperMonoBehaviour<PopUpWindowGenerator>
{
    [SerializeField] private GameObject dialogWindow = null;
    [SerializeField] private GameObject choosingWindow = null;
    [SerializeField] private GameObject loadingWindow = null;

    public void CreateDialogWindow(string textToShow)
    {
        Instantiate(dialogWindow).GetComponent<DialogWindow>().textMeshProToShow.text = textToShow;
    }


    public ModelChoosingWindow CreateModelChoosingWindow(ModelChoosingInfo modelInfo)
    {
        ModelChoosingWindow modelChoosingWindow = Instantiate(choosingWindow).GetComponent<ModelChoosingWindow>();
        modelChoosingWindow.Constructor(modelInfo);

        return modelChoosingWindow;
    }


    public LoadingWindow CreateLoadingWindow()
    {
        return loadingWindow.SpawnFromPool<LoadingWindow>();
    }
}
