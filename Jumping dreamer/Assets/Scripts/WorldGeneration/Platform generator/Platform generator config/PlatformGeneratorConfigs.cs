using UnityEngine;
using System;
using System.Text;
using Desdiene;

public class PlatformGeneratorConfigs
{
    private readonly float generationDelay = 0.425f;

    private PlatformGeneratorConfigs(PlatformConfigs platformConfigs)
    {
        PlatformConfigs = platformConfigs;
        Debug.Log($"Create new platformConfigs: {PlatformConfigs}");
        // todo: должно определяться в зависимости от настроек генерации
        TimePeriodForGeneratingPlatforms = GetTimePeriodForGeneratingPlatforms(PlatformConfigs);
    }


    public static PlatformGeneratorConfigs GetDefault()
    {
        return new PlatformGeneratorConfigs(PlatformConfigs.GetDefault());
    }


    public static PlatformGeneratorConfigs GetRandom()
    {
        return new PlatformGeneratorConfigs(PlatformConfigs.GetRandom());
    }


    public PlatformConfigs PlatformConfigs { get; private set; }
    public float TimePeriodForGeneratingPlatforms { get; private set; }


    /// <summary>
    /// Получить период времени создания платформ
    /// </summary>
    /// <returns></returns>
    public float GetTimePeriodForGeneratingPlatforms(PlatformConfigs platformConfigs)
    {
        float delay = generationDelay;

        if (platformConfigs.CreatingPlace == PlatformConfigsData.PlatformCreatingPlace.InRandomArea) delay -= 0.1f;

        if (platformConfigs.CauseOfDestroy == PlatformCauseOfDestroy.CauseOfDestroy.NoLifeTime)
        {
            delay -= 0.2f;
        }

        return delay;
    }
}


public class PlatformConfigs
{
    public PlatformMovingTypes[] MovingTypes { get; private set; }
    public IPlatformMotionConfig[] MovingTypeConfigs { get; private set; }
    public PlatformConfigsData.PlatformCreatingPlace CreatingPlace { get; private set; }
    public PlatformCauseOfDestroy.CauseOfDestroy CauseOfDestroy { get; private set; }


    public static PlatformConfigs GetDefault()
    {
        return new PlatformConfigs()
        {
            MovingTypes = new PlatformMovingTypes[] { global::PlatformMovingTypes.VerticalMotion },
            MovingTypeConfigs = new IPlatformMotionConfig[] {
                new VerticalMotionConfig(VerticalMotionConfig.MotionConfigs.Up) },
            CreatingPlace = PlatformConfigsData.PlatformCreatingPlace.InCentre,
            CauseOfDestroy = PlatformCauseOfDestroy.CauseOfDestroy.TopBorder
        };
    }

    public static PlatformConfigs GetRandom()
    {
        PlatformConfigsData platformConfigsData = new PlatformConfigsData();
        var platformMovingTypes = Randomizer.GetAllEnumValues<PlatformMovingTypes>();
        var platformMovingTypeConfigs = platformConfigsData.GetRandomPlatformMovingConfigs(platformMovingTypes);
        var platformCreatingPlace = platformConfigsData.GetRandomPlatformCreatingPlace(platformMovingTypes, platformMovingTypeConfigs);

        var platformCauseOfDestroy = platformConfigsData.GetRandomPlatformCauseOfDestroy(
            platformMovingTypes,
            platformMovingTypeConfigs,
            platformCreatingPlace);

        return new PlatformConfigs()
        {
            MovingTypes = platformMovingTypes,
            MovingTypeConfigs = platformMovingTypeConfigs,
            CreatingPlace = platformCreatingPlace,
            CauseOfDestroy = platformCauseOfDestroy
        };
    }

    public override string ToString()
    {
        StringBuilder platformMovingTypesConfigsBuilder = new StringBuilder();
        StringBuilder platformMovingTypesBuilder = new StringBuilder();

        Array.ForEach(MovingTypes, item => platformMovingTypesBuilder.Append(item + " "));
        Array.ForEach(MovingTypeConfigs, item => platformMovingTypesConfigsBuilder.Append(item.ToString() + " "));

        return string.Format($"PlatformMovingTypes: {platformMovingTypesBuilder}, PlatformMovingTypeConfigs: {platformMovingTypesConfigsBuilder}, PlatformCreatingPlace: {CreatingPlace}, PlatformCauseOfDestroy: {CauseOfDestroy}");
    }
}
