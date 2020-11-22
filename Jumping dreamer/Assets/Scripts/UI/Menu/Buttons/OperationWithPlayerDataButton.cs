using UnityEngine;
using UnityEngine.UI;

public abstract class OperationWithPlayerDataButton : MonoBehaviour
{
    public ConfirmationOperationWindow ConfirmationOperationWindow;

    private protected Button button;


    private void Start()
    {
        button = gameObject.GetComponent<Button>();

        PlayerDataModelController.Instance.OnResetPlayerData += ToggleButton;
        PlayerDataModelController.Instance.OnRestoreDataFromCloud += ToggleButton;

        ToggleButton();
    }


    private void OnDestroy()
    {
        PlayerDataModelController.Instance.OnResetPlayerData -= ToggleButton;
        PlayerDataModelController.Instance.OnRestoreDataFromCloud -= ToggleButton;
    }


    public virtual void DoOperationWithPlayerData()
    {
        ConfirmationOperationWindow.gameObject.SetActive(true);
    }


    private protected virtual void ToggleButton()
    {
        button.interactable = !PlayerDataModelController.IsPlayerDataHaveAlreadyDeletedOrRestored;
    }
}
