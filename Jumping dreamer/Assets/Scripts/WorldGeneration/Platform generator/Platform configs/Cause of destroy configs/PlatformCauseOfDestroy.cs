public class PlatformCauseOfDestroy
{
    public enum CauseOfDestroy
    {
        AsTimePasses,
        NoLifeTime,
        LateInitialization, // Определяется после создания создания вертикальной платформы
        TopBorder,
        BottomBorder
    }

    public CauseOfDestroy Value { get; }


    public PlatformCauseOfDestroy(CauseOfDestroy causesOfDestroy)
    {
        Value = causesOfDestroy;
    }
}
