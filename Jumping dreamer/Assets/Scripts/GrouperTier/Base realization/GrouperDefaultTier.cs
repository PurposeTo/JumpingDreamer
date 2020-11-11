/// <typeparam name="T">Данный хранимый тип</typeparam>
public abstract class GrouperDefaultTier<T> : IGrouperDefaultTier<T>
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