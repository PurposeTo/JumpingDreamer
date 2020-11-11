/// <typeparam name="T">Данный хранимый тип</typeparam>
public abstract class GrouperHigherTier<T, U> : GrouperDefaultTier<T>, IGrouperHigherTier<T>
{
    protected GrouperHigherTier(T value) : base(value) { }
}


/// <typeparam name="T">Данный хранимый тип</typeparam>
public interface IGrouperHigherTier<T> : IGrouperDefaultTier<T> { }
