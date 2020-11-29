using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class TimeScaler : Singleton<TimeScaler>
{
    public List<IPausable> AllPausers { get; private set; } = new List<IPausable>();

    /// <summary>
    /// Установить паузу игры
    /// </summary>
    public void SetPause(IPausable pausable)
    {
        if (!AllPausers.Contains(pausable)) AllPausers.Add(pausable);
        SetTimeScale();
    }


    private void SetTimeScale()
    {
        if (AllPausers.Any(pausable => pausable.IsPause)) Time.timeScale = 0f;
        else Time.timeScale = 1f;
    }
}