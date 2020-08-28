using System;
using UnityEngine;

public class DialogWindowGenerator : SingletonMonoBehaviour<DialogWindowGenerator>
{
    [SerializeField] private GameObject dialogWindow = null;

    public void CreateErrorWindow(string textToShow)
    {
        Instantiate(dialogWindow).GetComponent<DialogWindow>().textMeshProToShow.text = textToShow;
    }
}
