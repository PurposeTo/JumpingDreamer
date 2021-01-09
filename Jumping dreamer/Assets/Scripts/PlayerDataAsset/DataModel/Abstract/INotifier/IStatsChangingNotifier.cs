using System;

public interface IStatsChangingNotifier
{
    event Action OnNewScoreRecord;
}
