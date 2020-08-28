using UnityEngine;
using UnityEngine.UI;
using System;

public abstract class OperationWithPlayerDataButton : MonoBehaviour
{
    public ConfirmationOperationWindow ConfirmationOperationWindow;

    private Button button;


    private void Start()
    {
        button = gameObject.GetComponent<Button>();

        PlayerDataLocalStorageSafe.Instance.OnDeletePlayerData += ToggleButton;

        ToggleButton();
    }


    private void OnDestroy()
    {
        PlayerDataLocalStorageSafe.Instance.OnDeletePlayerData -= ToggleButton;
    }


    public virtual void ResetPlayerData()
    {
        ConfirmationOperationWindow.gameObject.SetActive(true);
    }


    private void ToggleButton()
    {
        button.interactable = !PlayerDataLocalStorageSafe.IsPlayerDataAlreadyReset;
    }
}
