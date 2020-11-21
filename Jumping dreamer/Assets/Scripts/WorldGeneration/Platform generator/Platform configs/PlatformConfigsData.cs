using System;
using System.Collections.Generic;
using System.Linq;

public class PlatformConfigsData
{
    public enum PlatformCreatingPlace
    {
        InRandomArea,
        InCentre,
        InHighestArea
    }


    public IPlatformMotionConfig[] GetRandomPlatformMovingConfigs(PlatformMovingTypes[] platformMovingTypes)
    {
        List<IPlatformMotionConfig> platformMovingConfigs = new List<IPlatformMotionConfig>();

        PlatformMotionConfigFactory platformMotionConfigFactory = new PlatformMotionConfigFactory();

        for (int i = 0; i < platformMovingTypes.Length; i++)
        {
            platformMovingConfigs.Add(platformMotionConfigFactory.GetRandomPlatformMotionConfig(platformMovingTypes[i]));
        }

        return platformMovingConfigs.ToArray();
    }


    public PlatformCreatingPlace GetRandomPlatformCreatingPlace(PlatformMovingTypes[] platformMovingTypes,
        IPlatformMotionConfig[] platformMovingTypeConfigs)
    {
        HashSet<PlatformCreatingPlace> availablePlatformCreatingPlaces = new HashSet<PlatformCreatingPlace>
        {
            PlatformCreatingPlace.InRandomArea
        };

        if (platformMovingTypes.Contains(PlatformMovingTypes.VerticalMotion))
        {
            VerticalMotionConfig verticalMotionConfig =
                (VerticalMotionConfig)platformMovingTypeConfigs.ToList().Find(x => x is VerticalMotionConfig);

            switch (verticalMotionConfig.Value)
            {
                case VerticalMotionConfig.VerticalMotionConfigs.Up:
                    availablePlatformCreatingPlaces.Add(PlatformCreatingPlace.InCentre);
                    break;
                case VerticalMotionConfig.VerticalMotionConfigs.Down:
                    availablePlatformCreatingPlaces.Add(PlatformCreatingPlace.InHighestArea);
                    break;
                case VerticalMotionConfig.VerticalMotionConfigs.Random:
                    availablePlatformCreatingPlaces.Add(PlatformCreatingPlace.InRandomArea);
                    break;
                default:
                    throw new System.Exception($"{verticalMotionConfig.Value} is unknown MotionConfig!");
            }

        }

        return GameLogic.GetRandomItem(availablePlatformCreatingPlaces.ToArray());
    }


    public PlatformCausesOfDestroy GetRandomPlatformCauseOfDestroy(
        PlatformMovingTypes[] platformMovingTypes,
        IPlatformMotionConfig[] platformMotionConfigs,
        PlatformCreatingPlace platformCreatingPlace)
    {
        HashSet<PlatformCausesOfDestroy> platformCauseOfDestroys = new HashSet<PlatformCausesOfDestroy>();

        if (platformCreatingPlace == PlatformCreatingPlace.InRandomArea)
        {
            platformCauseOfDestroys.Add(PlatformCausesOfDestroy.NoLifeTime);
        }

        bool isPlatformVerticalMotion = platformMovingTypes.Contains(PlatformMovingTypes.VerticalMotion);
        bool isPlatformCircularMotion = platformMovingTypes.Contains(PlatformMovingTypes.CircularMotion);

        if (isPlatformVerticalMotion)
        {
            VerticalMotionConfig verticalMotionConfig =
                (VerticalMotionConfig)platformMotionConfigs.ToList().Find(x => x is VerticalMotionConfig);

            platformCauseOfDestroys.Add(GetPlatformCauseOfDestroyByVerticalMotionConfig(verticalMotionConfig.Value));
        }
        else if (!isPlatformVerticalMotion && isPlatformCircularMotion)
        {
            platformCauseOfDestroys.Add(PlatformCausesOfDestroy.AsTimePasses);
        }

        return GameLogic.GetRandomItem(platformCauseOfDestroys.ToArray());
    }


    public PlatformCausesOfDestroy GetPlatformCauseOfDestroyByVerticalMotionConfig(VerticalMotionConfig.VerticalMotionConfigs verticalMotionConfig)
    {
        switch (verticalMotionConfig)
        {
            case VerticalMotionConfig.VerticalMotionConfigs.Up:
                return PlatformCausesOfDestroy.TopBorder;
            case VerticalMotionConfig.VerticalMotionConfigs.Down:
                return PlatformCausesOfDestroy.BottomBorder;
            case VerticalMotionConfig.VerticalMotionConfigs.Random:
                return PlatformCausesOfDestroy.LateInitialization;
            default:
                throw new Exception($"{verticalMotionConfig} is unknown motionConfig!");
        }
    }
}
