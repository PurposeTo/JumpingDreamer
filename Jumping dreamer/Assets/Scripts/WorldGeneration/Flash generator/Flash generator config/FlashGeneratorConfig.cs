using UnityEngine;

public class FlashGeneratorConfig
{
    public bool IsGenerating { get; private set; }
    public float TimePeriodForGeneratingFlashs { get; private set; }

    private readonly float minTimePeriodForGeneratingFlashs = 2f;
    private readonly float maxTimePeriodForGeneratingFlashs = 16f;


    private FlashGeneratorConfig(bool isGenerating)
    {
        IsGenerating = isGenerating;

        if (isGenerating) TimePeriodForGeneratingFlashs = Random.Range(minTimePeriodForGeneratingFlashs, maxTimePeriodForGeneratingFlashs);
        else TimePeriodForGeneratingFlashs = 0f;

        Debug.Log($"Create new FlashGeneratorConfig: {this}");
    }


    public static FlashGeneratorConfig GetDefault()
    {
        return new FlashGeneratorConfig(isGenerating: false);
    }


    public static FlashGeneratorConfig GetRandom()
    {
        return new FlashGeneratorConfig(isGenerating: true);
    }


    public override string ToString()
    {
        return string.Format($"FlashGeneratorConfig: IsGenerating = {IsGenerating}, TimePeriodForGeneratingFlashs: {TimePeriodForGeneratingFlashs}");
    }
}
