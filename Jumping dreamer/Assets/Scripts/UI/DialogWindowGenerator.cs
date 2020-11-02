using System;
using UnityEngine;

public class DialogWindowGenerator : SingletonMonoBehaviour<DialogWindowGenerator>
{
    [SerializeField] private GameObject dialogWindow = null;
    [SerializeField] private GameObject choosingWindow = null;

    public void CreateDialogWindow(string textToShow)
    {
        Instantiate(dialogWindow).GetComponent<DialogWindow>().textMeshProToShow.text = textToShow;
    }


    public void CreateChoosingWindow(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        Instantiate(choosingWindow).GetComponent<MixingModelsWindow>().Initialize(localModel, cloudModel);
    }
}
