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



public class VerticalMotionConfig : PlatformMotionConfig<VerticalMotionConfig.VerticalMotionConfigs>, IPlatformMotionConfig
{
    public enum VerticalMotionConfigs
    {
        Up,
        Down,
        Random
    }

    public VerticalMotionConfig() : base() { }

    public VerticalMotionConfig(VerticalMotionConfigs verticalMotionConfigs) : base(verticalMotionConfigs) { }

    protected override VerticalMotionConfigs[] ConcreteEnumValues { get; } = { VerticalMotionConfigs.Up, VerticalMotionConfigs.Down };
}


public class CircularMotionConfig : PlatformMotionConfig<CircularMotionConfig.CircularMotionConfigs>, IPlatformMotionConfig
{
    public enum CircularMotionConfigs
    {
        Left,
        Right,
        Random
    }

    public CircularMotionConfig() : base() { }

    public CircularMotionConfig(CircularMotionConfigs circularMotionConfigs) : base(circularMotionConfigs) { }

    protected override CircularMotionConfigs[] ConcreteEnumValues { get; } = { CircularMotionConfigs.Left, CircularMotionConfigs.Right };
}


/// <summary>
/// Необходим, что бы создать контейнер из элементов
/// </summary>
public interface IPlatformMotionConfig
{

}