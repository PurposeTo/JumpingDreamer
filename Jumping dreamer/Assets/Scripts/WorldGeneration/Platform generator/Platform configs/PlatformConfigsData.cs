using System;
using System.Collections.Generic;
using System.Linq;
using Desdiene;

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
                case VerticalMotionConfig.MotionConfigs.Up:
                    availablePlatformCreatingPlaces.Add(PlatformCreatingPlace.InCentre);
                    break;
                case VerticalMotionConfig.MotionConfigs.Down:
                    availablePlatformCreatingPlaces.Add(PlatformCreatingPlace.InHighestArea);
                    break;
                case VerticalMotionConfig.MotionConfigs.Random:
                    availablePlatformCreatingPlaces.Add(PlatformCreatingPlace.InRandomArea);
                    break;
                default:
                    throw new System.Exception($"{verticalMotionConfig.Value} is unknown MotionConfig!");
            }

        }

        return Randomizer.GetRandomItem(availablePlatformCreatingPlaces.ToArray());
    }


    public PlatformCauseOfDestroy.CauseOfDestroy GetRandomPlatformCauseOfDestroy(
        PlatformMovingTypes[] platformMovingTypes,
        IPlatformMotionConfig[] platformMotionConfigs,
        PlatformCreatingPlace platformCreatingPlace)
    {
        HashSet<PlatformCauseOfDestroy.CauseOfDestroy> platformCauseOfDestroys = new HashSet<PlatformCauseOfDestroy.CauseOfDestroy>();

        if (platformCreatingPlace == PlatformCreatingPlace.InRandomArea)
        {
            platformCauseOfDestroys.Add(PlatformCauseOfDestroy.CauseOfDestroy.NoLifeTime);
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
            platformCauseOfDestroys.Add(PlatformCauseOfDestroy.CauseOfDestroy.AsTimePasses);
        }

        return Randomizer.GetRandomItem(platformCauseOfDestroys.ToArray());
    }


    public PlatformCauseOfDestroy.CauseOfDestroy GetPlatformCauseOfDestroyByVerticalMotionConfig(VerticalMotionConfig.MotionConfigs verticalMotionConfig)
    {
        switch (verticalMotionConfig)
        {
            case VerticalMotionConfig.MotionConfigs.Up:
                return PlatformCauseOfDestroy.CauseOfDestroy.TopBorder;
            case VerticalMotionConfig.MotionConfigs.Down:
                return PlatformCauseOfDestroy.CauseOfDestroy.BottomBorder;
            case VerticalMotionConfig.MotionConfigs.Random:
                return PlatformCauseOfDestroy.CauseOfDestroy.LateInitialization;
            default:
                throw new Exception($"{verticalMotionConfig} is unknown motionConfig!");
        }
    }
}
