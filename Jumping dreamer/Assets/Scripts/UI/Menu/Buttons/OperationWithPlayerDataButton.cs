using UnityEngine;
using UnityEngine.UI;

public abstract class OperationWithPlayerDataButton : MonoBehaviour
{
    public ConfirmationOperationWindow ConfirmationOperationWindow;

    private protected Button button;


    private void Start()
    {
        button = gameObject.GetComponent<Button>();
    }


    public virtual void DoOperationWithPlayerData()
    {
        ConfirmationOperationWindow.gameObject.SetActive(true);
    }
}
