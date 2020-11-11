/// <typeparam name="T">Данный хранимый enum</typeparam>
public abstract class GrouperEnumDefaultTier<T> : GrouperDefaultTier<T>, IGrouperEnumDefaultTier<T>
    where T : System.Enum
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
    T[] AllEnumValues { get; }
}