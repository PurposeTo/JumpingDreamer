using UnityEngine;

public class FlashGeneratorConfig
{
    public bool IsGenerating { get; private set; }
    public float TimePeriodForGeneratingFlashs { get; private set; }

    private readonly float minTimePeriodForGeneratingFlashs = 2f;
    private readonly float maxTimePeriodForGeneratingFlashs = 12f;


    private FlashGeneratorConfig(CreatingValuesInClassMode creating)
    {
        switch (creating)
        {
            case CreatingValuesInClassMode.Random:
                InitializePropertiesByRandomValues();
                break;
            case CreatingValuesInClassMode.Default:
                InitializePropertiesByDefaultValues();
                break;
            default:
                Debug.LogError($"{creating} is unknown Creating!");
                InitializePropertiesByDefaultValues();
                break;
        }

        Debug.Log($"Create new FlashGeneratorConfig: {this}");
    }


    public static FlashGeneratorConfig GetDefault()
    {
        return new FlashGeneratorConfig(CreatingValuesInClassMode.Default);
    }


    public static FlashGeneratorConfig GetRandom()
    {
        return new FlashGeneratorConfig(CreatingValuesInClassMode.Random);
    }


    private void InitializePropertiesByDefaultValues()
    {
        IsGenerating = false;
        TimePeriodForGeneratingFlashs = 0f;
    }


    private void InitializePropertiesByRandomValues()
    {
        IsGenerating = true;
        TimePeriodForGeneratingFlashs = Random.Range(minTimePeriodForGeneratingFlashs, maxTimePeriodForGeneratingFlashs);
    }


    public override string ToString()
    {
        return string.Format($"FlashGeneratorConfig: IsGenerating = {IsGenerating}, TimePeriodForGeneratingFlashs: {TimePeriodForGeneratingFlashs}");
    }
}
