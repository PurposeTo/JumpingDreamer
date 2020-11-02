public class ResetPlayerDataButton : OperationWithPlayerDataButton
{
    public override void DoOperationWithPlayerData()
    {
        ConfirmationOperationWindow.Initialize("ConfirmResetDataKeyword", "DialogConfirmResetData", success =>
        {
            if (success)
            {
                PlayerDataModelController.Instance.DeletePlayerData();
                PopUpWindowGenerator.Instance.CreateDialogWindow(LocalizationManager.Instance.GetLocalizedValue("ProgressWasDeleted"));
            }
            else
            {    
                PopUpWindowGenerator.Instance.CreateDialogWindow(LocalizationManager.Instance.GetLocalizedValue("OperationNotConfirm"));
            }
        });

        base.DoOperationWithPlayerData();
    }
}
