using System;
using Debug = UnityEngine.Debug;

public class PlatformCauseOfDestroyCreator
{

    private float destroyHight;
    private float lifeTimeToDestroy;


    public Predicate<float> GetCauseOfDestroyByTime(PlatformConfigsData.PlatformCauseOfDestroy platformCauseOfDestroy)
    {

        switch (platformCauseOfDestroy)
        {
            case PlatformConfigsData.PlatformCauseOfDestroy.AsTimePasses:
                return GetAsTimePassesCauseOfDestroy();
            case PlatformConfigsData.PlatformCauseOfDestroy.NoLifeTime:
                return GetNoLifeTimeCauseOfDestroy();
            case PlatformConfigsData.PlatformCauseOfDestroy.VerticalCauseOfDestroy:
                throw new Exception($"For VerticalCauseOfDeathControl use GetCauseOfDestroyByHight!");

            default:
                throw new Exception($"{platformCauseOfDestroy} is unknown PlatformCauseOfDestroy!");
        }
    }


    public Predicate<float> GetCauseOfDestroyByHight(PlatformConfigsData.PlatformVerticalCauseOfDestroy varticalCauseOfDead)
    {
        switch (varticalCauseOfDead)
        {
            case PlatformConfigsData.PlatformVerticalCauseOfDestroy.TopBorder:
                return GetTopBorderCauseOfDestroy();
            case PlatformConfigsData.PlatformVerticalCauseOfDestroy.BottomBorder:
                return GetBottomBorderCauseOfDestroy();
            default:
                throw new Exception($"{varticalCauseOfDead} is unknown VerticalCauseOfDeathControl!");
        }
    }


    private Predicate<float> GetAsTimePassesCauseOfDestroy()
    {
        lifeTimeToDestroy = UnityEngine.Random.Range(PlatformGeneratorData.MinlifeTime, PlatformGeneratorData.MaxlifeTime);
        return GetAsTimeCauseOfDestroy();
    }


    private Predicate<float> GetNoLifeTimeCauseOfDestroy()
    {
        lifeTimeToDestroy = PlatformGeneratorData.MinlifeTime / 2.5f;
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
