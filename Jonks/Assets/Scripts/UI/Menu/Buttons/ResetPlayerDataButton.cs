public class ResetPlayerDataButton : OperationWithPlayerDataButton
{
    public override void ResetPlayerData()
    {
        ConfirmationOperationWindow.Initialize("ConfirmResetDataKeyword", "DialogConfirmResetData", success =>
        {
            if (success)
            {
                PlayerDataModelController.Instance.DeletePlayerData();
                // TODO Localization
                DialogWindowGenerator.Instance.CreateErrorWindow("Прогресс удален!");
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
