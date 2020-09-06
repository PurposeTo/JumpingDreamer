public class ResetPlayerDataButton : OperationWithPlayerDataButton
{
    public override void DoOperationWithPlayerData()
    {
        ConfirmationOperationWindow.Initialize("ConfirmResetDataKeyword", "DialogConfirmResetData", success =>
        {
            if (success)
            {
                PlayerDataModelController.Instance.DeletePlayerData();
                DialogWindowGenerator.Instance.CreateDialogWindow(LocalizationManager.Instance.GetLocalizedValue("ProgressWasDeleted"));
            }
            else
            {    
                DialogWindowGenerator.Instance.CreateDialogWindow(LocalizationManager.Instance.GetLocalizedValue("OperationNotConfirm"));
            }
        });

        base.DoOperationWithPlayerData();
    }
}
