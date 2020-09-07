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


    public MixingModelsWindow CreateChoosingWindow(PlayerDataModel localModel, PlayerDataModel cloudModel)
    {
        MixingModelsWindow mixingModelsWindow = Instantiate(choosingWindow).GetComponent<MixingModelsWindow>();
        mixingModelsWindow.Initialize(localModel, cloudModel);

        return mixingModelsWindow;
    }
}
