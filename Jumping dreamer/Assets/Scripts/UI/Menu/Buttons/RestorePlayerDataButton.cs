using UnityEngine;

public class RestorePlayerDataButton : OperationWithPlayerDataButton
{
    public override void DoOperationWithPlayerData()
    {
        ConfirmationOperationWindow.Initialize("ConfirmRestoreDataKeyword", "DialogConfirmRestoreData", success =>
        {
            if (success)
            {
                PlayerDataModelController.Instance.RestorePlayerDataFromCloud();
                PopUpWindowGenerator.Instance.CreateDialogWindow(LocalizationManager.Instance.GetLocalizedValue("ProgressWasRestored"));
            }
            else
            {
                PopUpWindowGenerator.Instance.CreateDialogWindow(LocalizationManager.Instance.GetLocalizedValue("OperationNotConfirm"));
            }
        });

        base.DoOperationWithPlayerData();
    }


    private protected override void ToggleButton()
    {
        button.interactable = !PlayerDataModelController.IsPlayerDataHaveAlreadyDeletedOrRestored &&
            (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork
            || Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork);
    }
}
