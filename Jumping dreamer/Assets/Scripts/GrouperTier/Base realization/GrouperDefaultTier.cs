/// <typeparam name="T">Данный хранимый тип</typeparam>
/// <typeparam name="U">Класс, который хранит хранимый тип</typeparam>
public abstract class GrouperDefaultTier<T, U> : IGrouperDefaultTier<T>
    where U : GrouperDefaultTier<T, U>
{
    public T Value { get; }

    protected GrouperDefaultTier(T value)
    {
        Value = value;
    }
}


/// <typeparam name="T">Данный хранимый тип</typeparam>
public interface IGrouperDefaultTier<T>
{
    T Value { get; }
}