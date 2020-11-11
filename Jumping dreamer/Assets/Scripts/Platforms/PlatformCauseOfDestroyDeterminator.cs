using System;
using Debug = UnityEngine.Debug;

public class PlatformCauseOfDestroyDeterminator
{

    private float destroyHight;
    private float lifeTimeToDestroy;


    public Predicate<float> GetCauseOfDestroyByTime(PlatformCauseOfDestroyByTime.PlatformCausesOfDestroyByTime platformCausesOfDestroyByTime)
    {
        switch (platformCausesOfDestroyByTime)
        {
            case PlatformCauseOfDestroyByTime.PlatformCausesOfDestroyByTime.AsTimePasses:
                return GetAsTimePassesCauseOfDestroy();
            case PlatformCauseOfDestroyByTime.PlatformCausesOfDestroyByTime.NoLifeTime:
                return GetNoLifeTimeCauseOfDestroy();
            default:
                throw new Exception($"{platformCausesOfDestroyByTime} is unknown PlatformCauseOfDestroy!");
        }
    }


    public Predicate<float> GetCauseOfDestroyByHight(PlatformCauseOfDestroyByHight.PlatformCausesOfDestroyByHight platformCausesOfDestroyByHight)
    {
        switch (platformCausesOfDestroyByHight)
        {
            case PlatformCauseOfDestroyByHight.PlatformCausesOfDestroyByHight.TopBorder:
                return GetTopBorderCauseOfDestroy();
            case PlatformCauseOfDestroyByHight.PlatformCausesOfDestroyByHight.BottomBorder:
                return GetBottomBorderCauseOfDestroy();
            default:
                throw new Exception($"{platformCausesOfDestroyByHight} is unknown VerticalCauseOfDeathControl!");
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
