/// <typeparam name="T">Данный хранимый тип</typeparam>
/// <typeparam name="U">Класс, который хранит хранимый тип</typeparam>
public abstract class GrouperHigherTier<T, U> : GrouperDefaultTier<T, U>, IGrouperHigherTier<T>
    where U : GrouperHigherTier<T, U>
{
    protected GrouperHigherTier(T value) : base(value) { }
}


/// <typeparam name="T">Данный хранимый тип</typeparam>
public interface IGrouperHigherTier<T> : IGrouperDefaultTier<T> { }
