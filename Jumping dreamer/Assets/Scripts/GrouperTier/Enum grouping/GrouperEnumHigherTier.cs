/// <typeparam name="T">Данный хранимый enum</typeparam>
/// <typeparam name="U">Класс, который хранит хранимый enum</typeparam>
public abstract class GrouperEnumHigherTier<T, U> : GrouperEnumDefaultTier<T, U>, IGrouperEnumHigherTier<T>
    where T : System.Enum
    where U : GrouperEnumHigherTier<T, U>
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
