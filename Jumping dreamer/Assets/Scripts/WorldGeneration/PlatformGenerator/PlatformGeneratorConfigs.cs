using Newtonsoft.Json;
using UnityEngine;

public enum CreatingValuesInClassMode
{
    Random,
    Default
}
public class PlatformGeneratorConfigs
{
    private PlatformGeneratorConfigs(CreatingValuesInClassMode creating)
    {
        switch (creating)
        {
            case CreatingValuesInClassMode.Random:
                PlatformConfigs = PlatformConfigs.GetRandom();
                break;
            case CreatingValuesInClassMode.Default:
                PlatformConfigs = PlatformConfigs.GetDefault();
                break;
            default:
                break;
        }
        
        Debug.Log($"Create new platformConfigs: {PlatformConfigs}");
        // todo: должно определяться в зависимости от настроек генерации
        TimePeriodForGeneratingPlatforms = GetTimePeriodForGeneratingPlatforms(PlatformConfigs);
    }


    public static PlatformGeneratorConfigs GetDefault()
    {
        return new PlatformGeneratorConfigs(CreatingValuesInClassMode.Default);
    }


    public static PlatformGeneratorConfigs GetRandom()
    {
        return new PlatformGeneratorConfigs(CreatingValuesInClassMode.Random);
    }


    public PlatformConfigs PlatformConfigs { get; private set; }
    public float TimePeriodForGeneratingPlatforms { get; private set; }


    /// <summary>
    /// Получить период времени создания платформ
    /// </summary>
    /// <returns></returns>
    public float GetTimePeriodForGeneratingPlatforms(PlatformConfigs platformConfigs)
    {
        float delay = 0.42f;

        if (platformConfigs.PlatformCreatingPlace == PlatformConfigsData.PlatformCreatingPlace.InRandomArea) delay -= 0.04f;
        if (platformConfigs.PlatformCauseOfDestroy == PlatformConfigsData.PlatformCauseOfDestroy.NoLifeTime) delay -= 0.1f;

        return delay;
    }
}


public class PlatformConfigs
{
    private PlatformConfigs(CreatingValuesInClassMode creating)
    {
        switch (creating)
        {
            case CreatingValuesInClassMode.Random:
                InitializePropertiesByRandomValues();
                break;
            case CreatingValuesInClassMode.Default:
                break;
            default:
                throw new System.Exception($"{creating} is unknown Creating!");
        }
    }

    public PlatformConfigsData.PlatformMovingType[] PlatformMovingTypes { get; private set; }
    public IPlatformMotionConfig[] PlatformMovingTypeConfigs { get; private set; }
    public PlatformConfigsData.PlatformCreatingPlace PlatformCreatingPlace { get; private set; }
    public PlatformConfigsData.PlatformCauseOfDestroy PlatformCauseOfDestroy { get; private set; }


    public static PlatformConfigs GetDefault()
    {
        return new PlatformConfigs(CreatingValuesInClassMode.Default)
        {
            PlatformMovingTypes = new PlatformConfigsData.PlatformMovingType[] { PlatformConfigsData.PlatformMovingType.VerticalMotion },
            PlatformMovingTypeConfigs = new IPlatformMotionConfig[] { VerticalMotionConfig.GetDefault() },
            PlatformCreatingPlace = PlatformConfigsData.PlatformCreatingPlace.InCentre,
            PlatformCauseOfDestroy = PlatformConfigsData.PlatformCauseOfDestroy.VerticalCauseOfDeathControl
        };
    }

    public static PlatformConfigs GetRandom()
    {
        return new PlatformConfigs(CreatingValuesInClassMode.Random);
    }

    public override string ToString()
    {
        return JsonConvert.SerializeObject(this);
    }


    private void InitializePropertiesByRandomValues()
    {
        PlatformConfigsData platformConfigsData = new PlatformConfigsData();
        PlatformMovingTypes = platformConfigsData.GetRandomPlatformMovingTypes();
        PlatformMovingTypeConfigs = platformConfigsData.GetPlatformMovingConfigs(PlatformMovingTypes);
        PlatformCreatingPlace = platformConfigsData.GetPlatformCreatingPlace(PlatformMovingTypes, PlatformMovingTypeConfigs);
        PlatformCauseOfDestroy = platformConfigsData.GetPlatformCauseOfDestroy(PlatformMovingTypes, PlatformCreatingPlace);
    }
}
