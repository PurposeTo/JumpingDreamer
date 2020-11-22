﻿using System;
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


    public ModelChoosingWindow CreateModelChoosingWindow(Action<LoadedPlayerDataModel> chooseTheModelCallback)
    {
        ModelChoosingWindow modelChoosingWindow = Instantiate(choosingWindow).GetComponent<ModelChoosingWindow>();
        modelChoosingWindow.Constructor(chooseTheModelCallback);
    }


    public LoadingWindow CreateLoadingWindow()
    {
        return GameLogic.SpawnFromPoolAndGetComponent<LoadingWindow>(this.loadingWindow, Vector3.zero, Quaternion.identity);
    }
}
