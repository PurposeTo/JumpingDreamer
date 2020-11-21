public enum PlatformCausesOfDestroy
{
    AsTimePasses,
    NoLifeTime,
    LateInitialization, // Определяется после создания создания вертикальной платформы
    TopBorder,
    BottomBorder
}


public class PlatformCauseOfDestroy
{
    public PlatformCausesOfDestroy Value { get; }


    public PlatformCauseOfDestroy(PlatformCausesOfDestroy causesOfDestroy)
    {
        Value = causesOfDestroy;
    }
}
