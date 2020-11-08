﻿using System.Collections.Generic;
using System.Linq;

public class PlatformConfigsData
{
    public enum PlatformMovingType
    {
        VerticalMotion,
        CircularMotion
    }
    private PlatformMovingType[] AllPlatformMovingTypes => new PlatformMovingType[]
    {
        PlatformMovingType.VerticalMotion,
        PlatformMovingType.CircularMotion
    };

    public enum PlatformCauseOfDestroy
    {
        AsTimePasses,
        NoLifeTime,
        VerticalCauseOfDestroy
    }

    public enum PlatformVerticalCauseOfDestroy
    {
        TopBorder,
        BottomBorder
    }

    public enum PlatformCreatingPlace
    {
        InRandomArea,
        InCentre,
        InHighestArea
    }


    /// <summary>
    /// Получить случайные типы платформ
    /// </summary>
    /// <returns></returns>
    public PlatformMovingType[] GetRandomPlatformMovingTypes()
    {
        return GameLogic.GetRandomItems(AllPlatformMovingTypes);
    }


    public IPlatformMotionConfig[] GetPlatformMovingConfigs(PlatformMovingType[] platformMovingTypes)
    {
        List<IPlatformMotionConfig> platformMovingConfigs = new List<IPlatformMotionConfig>();

        for (int i = 0; i < platformMovingTypes.Length; i++)
        {
            platformMovingConfigs.Add(PlatformMotionConfigFactory.GetPlatformMotionConfig(platformMovingTypes[i]));
        }

        return platformMovingConfigs.ToArray();
    }


    public PlatformCreatingPlace GetPlatformCreatingPlace(PlatformMovingType[] platformMovingTypes, 
        IPlatformMotionConfig[] platformMovingTypeConfigs)
    {
        HashSet<PlatformCreatingPlace> availablePlatformCreatingPlaces = new HashSet<PlatformCreatingPlace>();

        availablePlatformCreatingPlaces.Add(PlatformCreatingPlace.InRandomArea);

        if (platformMovingTypes.Contains(PlatformMovingType.VerticalMotion))
        {
            VerticalMotionConfig verticalMotionConfig = (VerticalMotionConfig)platformMovingTypeConfigs
                .ToList()
                .Find(platformMotionConfig => platformMotionConfig is VerticalMotionConfig);

            switch (verticalMotionConfig.MotionConfig)
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
                    throw new System.Exception($"{verticalMotionConfig.MotionConfig} is unknown MotionConfig!");
            }
        }

        return GameLogic.GetRandomItem(availablePlatformCreatingPlaces.ToArray());
    }


    public PlatformCauseOfDestroy GetPlatformCauseOfDestroy(PlatformMovingType[] platformMovingTypes, PlatformCreatingPlace platformCreatingPlace)
    {
        HashSet<PlatformCauseOfDestroy> platformCauseOfDestroys = new HashSet<PlatformCauseOfDestroy>();

        if (platformCreatingPlace == PlatformCreatingPlace.InRandomArea) platformCauseOfDestroys.Add(PlatformCauseOfDestroy.NoLifeTime);

        bool isPlatformVerticalMotion = platformMovingTypes.Contains(PlatformMovingType.VerticalMotion);
        bool isPlatformCircularMotion = platformMovingTypes.Contains(PlatformMovingType.CircularMotion);

        if (isPlatformVerticalMotion) platformCauseOfDestroys.Add(PlatformCauseOfDestroy.VerticalCauseOfDestroy);
        else if (!isPlatformVerticalMotion && isPlatformCircularMotion) platformCauseOfDestroys.Add(PlatformCauseOfDestroy.AsTimePasses);

        return GameLogic.GetRandomItem(platformCauseOfDestroys.ToArray());
    }
}
