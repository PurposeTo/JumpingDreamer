using UnityEngine;

public enum PlatformMovingTypes
{
    VerticalMotion,
    CircularMotion
}


public class PlatformMotionConfigFactory
{
    public IPlatformMotionConfig GetRandomPlatformMotionConfig(PlatformMovingTypes platformMovingType)
    {
        switch (platformMovingType)
        {
            case PlatformMovingTypes.VerticalMotion:
                return new VerticalMotionConfig();
            case PlatformMovingTypes.CircularMotion:
                return new CircularMotionConfig();
            default:
                Debug.LogError($"{platformMovingType} is unknown PlatformMovingType!");
                return new VerticalMotionConfig();
        }
    }
}


public class PlatformMotionConfig : GrouperEnumHigherTier<PlatformMovingTypes>
{
    public PlatformMotionConfig(PlatformMovingTypes platformMovingType) : base(platformMovingType) { }
}



public class VerticalMotionConfig
    : GrouperEnumLowerTierRandomable<VerticalMotionConfig.VerticalMotionConfigs, PlatformMovingTypes>,
    IPlatformMotionConfig
{
    public enum VerticalMotionConfigs
    {
        Up,
        Down,
        Random
    }

    public VerticalMotionConfig() : base(new PlatformMotionConfig(PlatformMovingTypes.VerticalMotion)) { }

    public VerticalMotionConfig(VerticalMotionConfigs verticalMotionConfigs) : base(verticalMotionConfigs,
        new PlatformMotionConfig(PlatformMovingTypes.VerticalMotion))
    { }

}


public class CircularMotionConfig :
    GrouperEnumLowerTierRandomable<CircularMotionConfig.CircularMotionConfigs, PlatformMovingTypes>, IPlatformMotionConfig
{
    public enum CircularMotionConfigs
    {
        Left,
        Right,
        Random
    }

    public CircularMotionConfig() : base(new PlatformMotionConfig(PlatformMovingTypes.CircularMotion)) { }

    public CircularMotionConfig(CircularMotionConfigs circularMotionConfigs) : base(circularMotionConfigs,
        new PlatformMotionConfig(PlatformMovingTypes.CircularMotion))
    { }
}


/// <summary>
/// Необходим, что бы создать контейнер из элементов
/// </summary>
public interface IPlatformMotionConfig : IGrouperEnumLowerTier<PlatformMovingTypes>
{

}