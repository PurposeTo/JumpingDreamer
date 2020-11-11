/// <typeparam name="T">Данный хранимый enum</typeparam>
public abstract class GrouperEnumHigherTier<T> : GrouperEnumDefaultTier<T>, IGrouperEnumHigherTier<T>
    where T : System.Enum
{
    protected GrouperEnumHigherTier(T value) : base(value) { }

    public override string ToString()
    {
        return Value.ToString();
    }
}


/// <typeparam name="T">Данный хранимый enum</typeparam>
public interface IGrouperEnumHigherTier<T> : IGrouperHigherTier<T>, IGrouperEnumDefaultTier<T>
    where T : System.Enum
{ }
