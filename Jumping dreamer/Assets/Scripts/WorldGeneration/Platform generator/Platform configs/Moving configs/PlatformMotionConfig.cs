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


public abstract class PlatformMotionConfig<T> where T : System.Enum
{
    public T Value { get; }

    protected abstract T[] ConcreteEnumValues { get; }

    public PlatformMotionConfig()
    {
        Value = GameLogic.GetRandomEnumItem<T>();
    }


    public PlatformMotionConfig(T Value)
    {
        this.Value = Value;
    }


    public T GetConcreteRandomEnumValue()
    {
        return GameLogic.GetRandomItem(ConcreteEnumValues);
    }
}



public class VerticalMotionConfig : PlatformMotionConfig<VerticalMotionConfig.MotionConfigs>, IPlatformMotionConfig
{
    public enum MotionConfigs
    {
        Up,
        Down,
        Random
    }

    public VerticalMotionConfig() : base() { }

    public VerticalMotionConfig(MotionConfigs verticalMotionConfigs) : base(verticalMotionConfigs) { }

    protected override MotionConfigs[] ConcreteEnumValues { get; } = { MotionConfigs.Up, MotionConfigs.Down };
}


public class CircularMotionConfig : PlatformMotionConfig<CircularMotionConfig.MotionConfigs>, IPlatformMotionConfig
{
    public enum MotionConfigs
    {
        Left,
        Right,
        Random
    }

    public CircularMotionConfig() : base() { }

    public CircularMotionConfig(MotionConfigs circularMotionConfigs) : base(circularMotionConfigs) { }

    protected override MotionConfigs[] ConcreteEnumValues { get; } = { MotionConfigs.Left, MotionConfigs.Right };
}


/// <summary>
/// Необходим, что бы создать контейнер из элементов
/// </summary>
public interface IPlatformMotionConfig
{

}