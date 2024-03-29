﻿using System;
using Debug = UnityEngine.Debug;

public class PlatformCauseOfDestroyDeterminator
{

    private float destroyHight;
    private float lifeTimeToDestroy;


    public Predicate<float> GetCauseOfDestroy(PlatformCauseOfDestroy.CauseOfDestroy causeOfDestroy)
    {
        switch (causeOfDestroy)
        {
            case PlatformCauseOfDestroy.CauseOfDestroy.AsTimePasses:
                return GetAsTimePassesCauseOfDestroy();
            case PlatformCauseOfDestroy.CauseOfDestroy.NoLifeTime:
                return GetNoLifeTimeCauseOfDestroy();
            case PlatformCauseOfDestroy.CauseOfDestroy.TopBorder:
                return GetTopBorderCauseOfDestroy();
            case PlatformCauseOfDestroy.CauseOfDestroy.BottomBorder:
                return GetBottomBorderCauseOfDestroy();
            default:
                throw new Exception($"{causeOfDestroy} is unknown PlatformCauseOfDestroy!");
        }
    }


    private Predicate<float> GetAsTimePassesCauseOfDestroy()
    {
        lifeTimeToDestroy = UnityEngine.Random.Range(PlatformGeneratorData.MinlifeTime, PlatformGeneratorData.MaxlifeTime);
        return GetAsTimeCauseOfDestroy();
    }


    private Predicate<float> GetNoLifeTimeCauseOfDestroy()
    {
        lifeTimeToDestroy = PlatformGeneratorData.MinlifeTime / 2f;
        return GetAsTimeCauseOfDestroy();
    }


    private Predicate<float> GetAsTimeCauseOfDestroy()
    {
        return (currentLifeTime) => currentLifeTime <= lifeTimeToDestroy;
    }


    private Predicate<float> GetTopBorderCauseOfDestroy()
    {
        float multiply = UnityEngine.Random.Range(2f / 3f, 1f);
        destroyHight = multiply * PlatformGeneratorData.AvailableHighestArea;

        return (currentHight) =>
        {
            return currentHight <= destroyHight;
        };
    }


    private Predicate<float> GetBottomBorderCauseOfDestroy()
    {
        destroyHight = Centre.CentreRadius * 2f;

        return (currentHight) =>
        {
            return currentHight >= destroyHight;
        };
    }
}
