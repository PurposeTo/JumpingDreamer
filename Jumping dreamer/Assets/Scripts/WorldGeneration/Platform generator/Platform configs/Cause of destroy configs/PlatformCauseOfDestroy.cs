public enum PlatformCausesOfDestroy
{
    ByTime,
    ByHight
}


public class PlatformCauseOfDestroy : GrouperEnumHigherTier<PlatformCausesOfDestroy, PlatformCauseOfDestroy>
{
    public PlatformCauseOfDestroy(PlatformCausesOfDestroy platformCausesOfDestroy) : base(platformCausesOfDestroy) { }
}


public class PlatformCauseOfDestroyByTime
    : GrouperEnumLowerTierRandomable<PlatformCauseOfDestroyByTime.PlatformCausesOfDestroyByTime, PlatformCauseOfDestroyByTime, PlatformCausesOfDestroy>,
    IPlatformCauseOfDestroyConfigs
{
    public enum PlatformCausesOfDestroyByTime
    {
        AsTimePasses,
        NoLifeTime
    }


    public PlatformCauseOfDestroyByTime() : base(new PlatformCauseOfDestroy(PlatformCausesOfDestroy.ByTime)) { }

    public PlatformCauseOfDestroyByTime(PlatformCausesOfDestroyByTime platformCauseOfDestroyConfigsByTime)
        : base(platformCauseOfDestroyConfigsByTime,
            new PlatformCauseOfDestroy(PlatformCausesOfDestroy.ByTime))
    { }
}


public class PlatformCauseOfDestroyByHight
    : GrouperEnumLowerTier<PlatformCauseOfDestroyByHight.PlatformCausesOfDestroyByHight, PlatformCauseOfDestroyByHight, PlatformCausesOfDestroy>,
    IPlatformCauseOfDestroyConfigs
{
    public enum PlatformCausesOfDestroyByHight
    {
        WaitingForInitializate, // Определение причины уничтожения произойдет внутри VerticalMotion
        TopBorder,
        BottomBorder
    }

    public PlatformCauseOfDestroyByHight(PlatformCausesOfDestroyByHight platformCausesOfDestroyByHight)
        : base(platformCausesOfDestroyByHight,
            new PlatformCauseOfDestroy(PlatformCausesOfDestroy.ByHight))
    { }
}


/// <summary>
/// Необходим, что бы создать контейнер из элементов
/// </summary>
public interface IPlatformCauseOfDestroyConfigs : IGrouperEnumLowerTier<PlatformCausesOfDestroy>
{

}
