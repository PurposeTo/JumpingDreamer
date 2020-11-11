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
            VerticalMotionConfig verticalMotionConfig = (VerticalMotionConfig)platformMovingTypeConfigs
                .ToList()
                .Find(platformMotionConfig => platformMotionConfig is VerticalMotionConfig);

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


    public IPlatformCauseOfDestroyConfigs GetRandomPlatformCauseOfDestroy(PlatformMovingTypes[] platformMovingTypes, PlatformCreatingPlace platformCreatingPlace)
    {
        HashSet<IPlatformCauseOfDestroyConfigs> platformCauseOfDestroys = new HashSet<IPlatformCauseOfDestroyConfigs>();

        if (platformCreatingPlace == PlatformCreatingPlace.InRandomArea)
        {
            platformCauseOfDestroys.Add(new PlatformCauseOfDestroyConfigsByTime(PlatformCauseOfDestroyConfigsByTime.PlatformCausesOfDestroyByTime.NoLifeTime));
        }

        bool isPlatformVerticalMotion = platformMovingTypes.Contains(PlatformMovingTypes.VerticalMotion);
        bool isPlatformCircularMotion = platformMovingTypes.Contains(PlatformMovingTypes.CircularMotion);

        if (isPlatformVerticalMotion)
        {
            platformCauseOfDestroys.Add(new PlatformCauseOfDestroyConfigsByHight(PlatformCauseOfDestroyConfigsByHight.PlatformCausesOfDestroyByHight.WaitingForInitializate));
        }
        else if (!isPlatformVerticalMotion && isPlatformCircularMotion)
        {
            platformCauseOfDestroys.Add(new PlatformCauseOfDestroyConfigsByTime(PlatformCauseOfDestroyConfigsByTime.PlatformCausesOfDestroyByTime.AsTimePasses));
        }

        return GameLogic.GetRandomItem(platformCauseOfDestroys.ToArray());
    }
}
