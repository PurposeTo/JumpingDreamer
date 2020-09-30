public abstract class PlatformMotionConfigFactory
{
    public static IPlatformMotionConfig GetPlatformMotionConfig(PlatformConfigsData.PlatformMovingType platformMovingType)
    {
        switch (platformMovingType)
        {
            case PlatformConfigsData.PlatformMovingType.VerticalMotion:
                return new VerticalMotionConfig();
            case PlatformConfigsData.PlatformMovingType.CircularMotion:
                return new CircularMotionConfig();
            default:
                throw new System.Exception($"{platformMovingType} is unknown PlatformMovingType!");
        }
    }
}


public interface IPlatformMotionConfig
{

}


public abstract class PlatformMotionConfig<T> where T : System.Enum
{
    public PlatformMotionConfig()
    {
        MotionConfig = GameLogic.GetRandomItem(AllMotionConfigs);
    }

    public T MotionConfig { get; private protected set; }

    private protected abstract T[] AllMotionConfigs { get; }
}


public class VerticalMotionConfig : PlatformMotionConfig<VerticalMotionConfig.VerticalMotionConfigs>, IPlatformMotionConfig
{
    public enum VerticalMotionConfigs
    {
        Up,
        Down,
        Random
    }

    private protected override VerticalMotionConfigs[] AllMotionConfigs => new VerticalMotionConfigs[]
    {
        VerticalMotionConfigs.Up,
        VerticalMotionConfigs.Down,
        VerticalMotionConfigs.Random
    };

    public static VerticalMotionConfig GetDefault()
    {
        // Да, выполняется переопределение. Это костыль.
        return new VerticalMotionConfig()
        {
            MotionConfig = VerticalMotionConfigs.Up
        };
    }
}


public class CircularMotionConfig : PlatformMotionConfig<CircularMotionConfig.CircularMotionConfigs>, IPlatformMotionConfig
{
    public enum CircularMotionConfigs
    {
        Left,
        Right,
        Random
    }

    private protected override CircularMotionConfigs[] AllMotionConfigs => new CircularMotionConfigs[]
    {
        CircularMotionConfigs.Left,
        CircularMotionConfigs.Right,
        CircularMotionConfigs.Random
    };
}
