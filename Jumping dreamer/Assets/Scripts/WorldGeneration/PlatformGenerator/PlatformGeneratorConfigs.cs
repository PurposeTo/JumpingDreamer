using Newtonsoft.Json;
using UnityEngine;

public enum Creating
{
    Random,
    Default
}
public class PlatformGeneratorConfigs
{
    public PlatformGeneratorConfigs(Creating creating)
    {
        switch (creating)
        {
            case Creating.Random:
                PlatformConfigs = PlatformConfigs.GetRandom();
                break;
            case Creating.Default:
                PlatformConfigs = PlatformConfigs.GetDefault();
                break;
            default:
                break;
        }
        
        Debug.Log($"Create new platformConfigs: {PlatformConfigs}");
        // todo: должно определяться в зависимости от настроек генерации
        TimePeriodForGeneratingPlatforms = 0.25f; // = platformConfigsData.GetTimePeriodForGeneratingPlatforms(); 
    }

    public PlatformConfigs PlatformConfigs { get; private set; }
    public float TimePeriodForGeneratingPlatforms { get; private set; }
}


public class PlatformConfigs
{
    private PlatformConfigs(Creating creating)
    {
        switch (creating)
        {
            case Creating.Random:
                InitializePropertiesByRandomValues();
                break;
            case Creating.Default:
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
        return new PlatformConfigs(Creating.Default)
        {
            PlatformMovingTypes = new PlatformConfigsData.PlatformMovingType[] { PlatformConfigsData.PlatformMovingType.VerticalMotion },
            PlatformMovingTypeConfigs = new IPlatformMotionConfig[] { VerticalMotionConfig.GetDefault() },
            PlatformCreatingPlace = PlatformConfigsData.PlatformCreatingPlace.InCentre,
            PlatformCauseOfDestroy = PlatformConfigsData.PlatformCauseOfDestroy.VerticalCauseOfDeathControl
        };
    }

    public static PlatformConfigs GetRandom()
    {
        return new PlatformConfigs(Creating.Random);
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
