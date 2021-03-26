using Desdiene.Time_scaler.Base;
using UnityEngine;

namespace Desdiene.Time_control
{
    internal sealed class GlobalTimeScaler : TimeScaler
    {
        public override float TimeScale { get => Time.timeScale; protected set { Time.timeScale = value; } }
    }
}
