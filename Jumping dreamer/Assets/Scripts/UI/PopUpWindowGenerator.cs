using System;
using UnityEngine;

public class PopUpWindowGenerator : SingletonMonoBehaviour<PopUpWindowGenerator>
{
    [SerializeField] private GameObject dialogWindow = null;
    [SerializeField] private GameObject choosingWindow = null;
    [SerializeField] private GameObject loadingWindow = null;

    public void CreateDialogWindow(string textToShow)
    {
        Instantiate(dialogWindow).GetComponent<DialogWindow>().textMeshProToShow.text = textToShow;
    }


    public void CreateChoosingWindow(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        Instantiate(choosingWindow).GetComponent<MixingModelsWindow>().Initialize(localModel, cloudModel);
    }


    public LoadingWindow CreateLoadingWindow()
    {
        return GameLogic.SpawnFromPoolAndGetComponent<LoadingWindow>(this.loadingWindow, Vector3.zero, Quaternion.identity);
    }
}
