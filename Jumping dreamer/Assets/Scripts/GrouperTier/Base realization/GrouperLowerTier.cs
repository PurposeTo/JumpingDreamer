/// <typeparam name="T">Данный хранимый тип</typeparam>
/// <typeparam name="U">Класс, который хранит данный хранимый тип</typeparam>
/// <typeparam name="V">Родительский хранимый тип</typeparam>
public abstract class GrouperLowerTier<T, U, V> : GrouperDefaultTier<T, U>, IGrouperLowerTier<T, V> 
    where U : GrouperLowerTier<T, U, V>
{
    public IGrouperHigherTier<V> ParentTier { get; }

    public GrouperLowerTier(T value, IGrouperHigherTier<V> parentTier) : base(value)
    {
        ParentTier = parentTier ?? throw new System.ArgumentNullException(nameof(parentTier));
    }
}


/// <typeparam name="T">Данный хранимый тип</typeparam>
/// <typeparam name="V">Родительский хранимый тип</typeparam>
public interface IGrouperLowerTier<T, V>
{
    IGrouperHigherTier<V> ParentTier { get; }
}
