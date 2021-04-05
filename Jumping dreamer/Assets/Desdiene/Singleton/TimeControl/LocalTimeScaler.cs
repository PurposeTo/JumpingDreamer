using Desdiene.Time_scaler.Base;
using UnityEngine;

namespace Desdiene.TimeControl
{
    public class LocalTimeScaler : TimeScaler
    {
        public override float TimeScale { get => timeScale; protected set { timeScale = value; } }
        private float timeScale = 1f; // По умолчанию время идет
    }
}
