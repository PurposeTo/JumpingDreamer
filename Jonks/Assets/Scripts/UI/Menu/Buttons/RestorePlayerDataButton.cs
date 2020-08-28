public class RestorePlayerDataButton : OperationWithPlayerDataButton
{
    public override void ResetPlayerData()
    {
        ConfirmationOperationWindow.Initialize("ConfirmRestoreDataKeyword", "DialogConfirmRestoreData", success =>
        {
            if (success)
            {
                PlayerDataModelController.Instance.RestorePlayerDataFromCloud();
                // TODO Localization
                DialogWindowGenerator.Instance.CreateErrorWindow("Прогресс восстановлен!");
            }
            else
            {
                // TODO Localization
                DialogWindowGenerator.Instance.CreateErrorWindow("Операция не была подтверждена!");
            }
        });

        base.ResetPlayerData();
    }
}
