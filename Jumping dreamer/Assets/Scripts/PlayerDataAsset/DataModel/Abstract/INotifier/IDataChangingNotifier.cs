using System;

public interface IDataChangingNotifier
{
    event Action OnDataReset;
    IStatsChangingNotifier StatsChangingNotifier { get; }
}
