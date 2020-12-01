using TMPro;

public class StatsScreen : SuperMonoBehaviour
{
    private string maxCollectedStars;
    private string maxEarnedScore;
    private string maxScoreMultiplierValue;
    private string maxLifeTime;
    private string totalLifeTime;

    private string totalStars;

    private TextMeshProUGUI recordText;


    protected override void AwakeWrapped()
    {
        recordText = gameObject.GetComponent<TextMeshProUGUI>();
    }


    protected override void OnEnableWrapped()
    {
        PlayerDataModelController.InitializedInstance += (instance) =>
        {
            IGetDataModel dataModel = instance.GetGettableDataModel();

            SetStatsText(dataModel.PlayerStats);
            SetInGamePurchasesText(dataModel.PlayerInGamePurchases);

            SetRecordsText();
        };
    }


    private void SetRecordsText()
    {
        recordText.text =
            $"{maxCollectedStars}\n" +
            $"{maxEarnedScore}\n" +
            $"{maxScoreMultiplierValue}\n" +
            $"{maxLifeTime}\n" +
            $"{totalLifeTime}\n" +
            $"{totalStars}\n";
    }


    private void SetStatsText(IGetStatsData statsData)
    {
        maxCollectedStars = $"Max collected stars: {statsData.MaxCollectedStars}";
        maxEarnedScore = $"Max earned score: {statsData.MaxEarnedScore}";
        maxScoreMultiplierValue = $"Max score multiplier: {statsData.MaxScoreMultiplierValue}";
        maxLifeTime = $"Max life time: {statsData.MaxLifeTime}";
        totalLifeTime = $"Total life time: {statsData.TotalLifeTime}";
    }


    private void SetInGamePurchasesText(IGetInGamePurchases InGamePurchases)
    {
        totalStars = $"Total stars: {InGamePurchases.TotalStars}";
    }
}