/// <typeparam name="T">Данный хранимый enum</typeparam>
/// <typeparam name="U">Класс, который хранит данный хранимый enum</typeparam>
/// <typeparam name="V">Родительский хранимый enum</typeparam>
public abstract class GrouperEnumLowerTier<T, U, V> : GrouperEnumDefaultTier<T, U>, IGrouperEnumLowerTier<V>, IGrouperEnumDefaultTier<T>
    where T : System.Enum
    where U : GrouperEnumLowerTier<T, U, V>
    where V : System.Enum
{
    public IGrouperEnumHigherTier<V> ParentTier { get; }

    protected GrouperEnumLowerTier(T value, IGrouperEnumHigherTier<V> parentTier) : base(value)
    {
        ParentTier = parentTier ?? throw new System.ArgumentNullException(nameof(parentTier));
    }


    public bool IsBelongToHigherNode(V HigherTierValue)
    {
        //UnityEngine.Debug.Log($"КРЯ! {HigherTierValue} equals {ParentTier.Value}. It's {HigherTierValue.Equals(ParentTier.Value)}!");
        return HigherTierValue.Equals(ParentTier.Value);
    }


    public bool TryToDownCastTier<D>(out D downcastedTier) where D : IGrouperEnumLowerTier<V>
    {
        downcastedTier = default;

        if (this is U && this is D _downcastableTier)
        {
            if (IsBelongToHigherNode(_downcastableTier.ParentTier.Value))
            {
                downcastedTier = _downcastableTier;
                return true;
            }
            else
            {
                UnityEngine.Debug.LogWarning($"{this.GetType()} is Belong to the node {_downcastableTier.ParentTier.Value}, but not belong to {typeof(U)}! Check relations!");
                return false;
            }
        }
        else
        {
            UnityEngine.Debug.Log($"{this.GetType()} is not belong to {typeof(U)}! Check relations!");
            return false;
        }
    }

    public D DownCastTier<D>() where D : IGrouperEnumLowerTier<V>
    {
        if (TryToDownCastTier(out D _downcastableTier))
        {
            return _downcastableTier;
        }
        else
        {
            return default;
        }
    }
}

/// <typeparam name="T">Данный хранимый enum</typeparam>
/// <typeparam name="U">Класс, который хранит данный хранимый enum</typeparam>
/// <typeparam name="V">Родительский хранимый enum</typeparam>
public abstract class GrouperEnumLowerTierRandomable<T, U, V> : GrouperEnumLowerTier<T, U, V>, IGrouperEnumLowerTier<V>, IGrouperEnumDefaultTier<T>
    where T : System.Enum
    where U : GrouperEnumLowerTierRandomable<T, U, V>
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

    bool IsBelongToHigherNode(V HigherTierValue);

    bool TryToDownCastTier<U>(out U downcastedTier) where U : IGrouperEnumLowerTier<V>;

    D DownCastTier<D>() where D : IGrouperEnumLowerTier<V>;
}
