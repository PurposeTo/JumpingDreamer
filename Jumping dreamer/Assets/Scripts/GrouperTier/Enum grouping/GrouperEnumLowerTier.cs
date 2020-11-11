/// <typeparam name="T">Данный хранимый enum</typeparam>
/// <typeparam name="V">Родительский хранимый enum</typeparam>
public abstract class GrouperEnumLowerTier<T, V> : GrouperEnumDefaultTier<T>, IGrouperEnumLowerTier<V>, IGrouperEnumDefaultTier<T>
    where T : System.Enum
    where V : System.Enum
{
    public IGrouperEnumHigherTier<V> ParentTier { get; }

    protected GrouperEnumLowerTier(T value, IGrouperEnumHigherTier<V> parentTier) : base(value)
    {
        ParentTier = parentTier ?? throw new System.ArgumentNullException(nameof(parentTier));
    }
}

/// <typeparam name="T">Данный хранимый enum</typeparam>
/// <typeparam name="V">Родительский хранимый enum</typeparam>
public abstract class GrouperEnumLowerTierRandomable<T, V> : GrouperEnumLowerTier<T, V>, IGrouperEnumLowerTier<V>, IGrouperEnumDefaultTier<T>
    where T : System.Enum
    where V : System.Enum
{

    protected GrouperEnumLowerTierRandomable(IGrouperEnumHigherTier<V> parentTier) : this(GameLogic.GetRandomEnumItem<T>(), parentTier) { }

    protected GrouperEnumLowerTierRandomable(T value, IGrouperEnumHigherTier<V> parentTier) : base(value, parentTier) { }
}


/// <typeparam name="V">Родительский хранимый enum</typeparam>
public interface IGrouperEnumLowerTier<V>
    where V : System.Enum
{
    IGrouperEnumHigherTier<V> ParentTier { get; }
}
