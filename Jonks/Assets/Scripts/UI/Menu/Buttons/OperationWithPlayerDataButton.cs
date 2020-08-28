using UnityEngine;
using UnityEngine.UI;

public abstract class OperationWithPlayerDataButton : MonoBehaviour
{
    public ConfirmationOperationWindow ConfirmationOperationWindow;

    private Button button;


    private void Start()
    {
        button = gameObject.GetComponent<Button>();

        PlayerDataModelController.Instance.OnDeletePlayerData += ToggleButton;

        ToggleButton();
    }


    private void OnDestroy()
    {
        PlayerDataModelController.Instance.OnDeletePlayerData -= ToggleButton;
    }


    public virtual void ResetPlayerData()
    {
        ConfirmationOperationWindow.gameObject.SetActive(true);
    }


    private void ToggleButton()
    {
        button.interactable = !PlayerDataModelController.IsPlayerDataAlreadyReset;
    }
}
