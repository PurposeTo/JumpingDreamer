using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class StatsView : SuperMonoBehaviour
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
            IDataGetter data = instance.DataInteraction.Getter;

            SetStatsText(data.Stats);
            SetInGamePurchasesText(data.InGamePurchases);

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


    private void SetStatsText(IStatsGetter stats)
    {
        maxCollectedStars = $"Max collected stars: {stats.RecordCollectedStars}";
        maxEarnedScore = $"Max earned score: {stats.RecordEarnedScore}";
        maxScoreMultiplierValue = $"Max score multiplier: {stats.RecordScoreMultiplierValue}";
        maxLifeTime = $"Max life time: {stats.RecordLifeTime}";
        totalLifeTime = $"Total life time: {stats.TotalLifeTime}";
    }


    private void SetInGamePurchasesText(IInGamePurchasesGetter InGamePurchases)
    {
        totalStars = $"Total stars: {InGamePurchases.TotalStars}";
    }
}