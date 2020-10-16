using UnityEngine;
using System;
using System.Text;

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
        if (platformConfigs.PlatformCauseOfDestroy == PlatformConfigsData.PlatformCauseOfDestroy.NoLifeTime) delay -= 0.15f;

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
                InitializePropertiesByDefaultValues();
                break;
            default:
                Debug.LogError($"{creating} is unknown Creating!");
                InitializePropertiesByDefaultValues();
                break;
        }
    }

    public PlatformConfigsData.PlatformMovingType[] PlatformMovingTypes { get; private set; }
    public IPlatformMotionConfig[] PlatformMovingTypeConfigs { get; private set; }
    public PlatformConfigsData.PlatformCreatingPlace PlatformCreatingPlace { get; private set; }
    public PlatformConfigsData.PlatformCauseOfDestroy PlatformCauseOfDestroy { get; private set; }


    public static PlatformConfigs GetDefault()
    {
        return new PlatformConfigs(CreatingValuesInClassMode.Default);
    }

    public static PlatformConfigs GetRandom()
    {
        return new PlatformConfigs(CreatingValuesInClassMode.Random);
    }

    public override string ToString()
    {
        StringBuilder platformMovingTypesConfigsBuilder = new StringBuilder();
        StringBuilder platformMovingTypesBuilder = new StringBuilder();

        Array.ForEach(PlatformMovingTypes, item => platformMovingTypesBuilder.Append(item + " "));
        Array.ForEach(PlatformMovingTypeConfigs, item => platformMovingTypesConfigsBuilder.Append(item.GetDebugEnumValue() + " "));

        return string.Format($"PlatformMovingTypes: {platformMovingTypesBuilder}, PlatformMovingTypeConfigs: {platformMovingTypesConfigsBuilder}, PlatformCreatingPlace: {PlatformCreatingPlace}, PlatformCauseOfDestroy: {PlatformCauseOfDestroy}");
    }


    private void InitializePropertiesByDefaultValues()
    {
        PlatformMovingTypes = new PlatformConfigsData.PlatformMovingType[] { PlatformConfigsData.PlatformMovingType.VerticalMotion };
        PlatformMovingTypeConfigs = new IPlatformMotionConfig[] { VerticalMotionConfig.GetDefault() };
        PlatformCreatingPlace = PlatformConfigsData.PlatformCreatingPlace.InCentre;
        PlatformCauseOfDestroy = PlatformConfigsData.PlatformCauseOfDestroy.VerticalCauseOfDeathControl;
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
