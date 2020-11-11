/// <typeparam name="T">Данный хранимый enum</typeparam>
/// <typeparam name="U">Класс, который хранит enum</typeparam>
public abstract class GrouperEnumDefaultTier<T, U> : GrouperDefaultTier<T, U>, IGrouperEnumDefaultTier<T>
    where T : System.Enum
    where U : GrouperEnumDefaultTier<T, U>
{
    public T[] AllEnumValues { get; }

    protected GrouperEnumDefaultTier(T value) : base(value)
    {
        AllEnumValues = GameLogic.GetAllEnumValues<T>();
    }
}


/// <typeparam name="T">Данный хранимый enum</typeparam>
public interface IGrouperEnumDefaultTier<T> : IGrouperDefaultTier<T>
    where T : System.Enum
{
    new T Value { get; }

    T[] AllEnumValues { get; }
}