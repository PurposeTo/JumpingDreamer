public class RestorePlayerDataButton : OperationWithPlayerDataButton
{
    public override void ResetPlayerData()
    {
        ConfirmationOperationWindow.Initialize("ConfirmDeleteData", "KeyDialog", success =>
        {
            if (success)
            {
                PlayerDataSynchronizer.RestorePlayerDataFromCloud();
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
