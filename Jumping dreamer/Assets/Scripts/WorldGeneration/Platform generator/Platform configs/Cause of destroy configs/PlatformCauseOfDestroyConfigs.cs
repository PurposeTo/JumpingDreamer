public enum PlatformCausesOfDestroy
{
    ByTime,
    ByHight
}


public class PlatformCauseOfDestroyConfigs : GrouperEnumHigherTier<PlatformCausesOfDestroy>
{
    public PlatformCauseOfDestroyConfigs(PlatformCausesOfDestroy platformCausesOfDestroy) : base(platformCausesOfDestroy) { }
}


public class PlatformCauseOfDestroyConfigsByTime
    : GrouperEnumLowerTierRandomable<PlatformCauseOfDestroyConfigsByTime.PlatformCausesOfDestroyByTime, PlatformCausesOfDestroy>,
    IPlatformCauseOfDestroyConfigs
{
    public enum PlatformCausesOfDestroyByTime
    {
        AsTimePasses,
        NoLifeTime
    }


    public PlatformCauseOfDestroyConfigsByTime() : base(new PlatformCauseOfDestroyConfigs(PlatformCausesOfDestroy.ByTime)) { }

    public PlatformCauseOfDestroyConfigsByTime(PlatformCausesOfDestroyByTime platformCauseOfDestroyConfigsByTime)
        : base(platformCauseOfDestroyConfigsByTime,
            new PlatformCauseOfDestroyConfigs(PlatformCausesOfDestroy.ByTime))
    { }
}


public class PlatformCauseOfDestroyConfigsByHight
    : GrouperEnumLowerTier<PlatformCauseOfDestroyConfigsByHight.PlatformCausesOfDestroyByHight, PlatformCausesOfDestroy>,
    IPlatformCauseOfDestroyConfigs
{
    public enum PlatformCausesOfDestroyByHight
    {
        WaitingForInitializate, // Определение причины уничтожения произойдет внутри VerticalMotion
        TopBorder,
        BottomBorder
    }

    public PlatformCauseOfDestroyConfigsByHight(PlatformCausesOfDestroyByHight platformCausesOfDestroyByHight)
        : base(platformCausesOfDestroyByHight,
            new PlatformCauseOfDestroyConfigs(PlatformCausesOfDestroy.ByHight))
    { }
}


/// <summary>
/// Необходим, что бы создать контейнер из элементов
/// </summary>
public interface IPlatformCauseOfDestroyConfigs : IGrouperEnumLowerTier<PlatformCausesOfDestroy>
{

}
