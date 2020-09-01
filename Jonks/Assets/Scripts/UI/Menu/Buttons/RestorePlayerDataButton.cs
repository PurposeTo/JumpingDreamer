public class RestorePlayerDataButton : OperationWithPlayerDataButton
{
    public override void DoOperationWithPlayerData()
    {
        ConfirmationOperationWindow.Initialize("ConfirmRestoreDataKeyword", "DialogConfirmRestoreData", success =>
        {
            if (success)
            {
                PlayerDataModelController.Instance.RestorePlayerDataFromCloud();
                DialogWindowGenerator.Instance.CreateDialogWindow(LocalizationManager.Instance.GetLocalizedValue("ProgressWasRestored"));
            }
            else
            {
                DialogWindowGenerator.Instance.CreateDialogWindow(LocalizationManager.Instance.GetLocalizedValue("OperationNotConfirm"));
            }
        });

        base.DoOperationWithPlayerData();
    }
}
