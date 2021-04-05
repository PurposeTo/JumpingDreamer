using UnityEngine;

namespace Desdiene.Time_scaler.Base
{
    public abstract class TimeScaler
    {
        public TimeScaler()
        {
            SaveTimeScaleValue();
        }

        public abstract float TimeScale { get; protected set; }
        private float timeScaleSaved; // Сохраненное значение скорости времени

        private bool isPause = false; // По умолчанию паузы нет
        public bool IsPause => isPause;

        public void SetPause(bool pause)
        {
            isPause = pause;
            SetTimeScaleViaPause();
        }

        public void SetTimeScale(float timeScale)
        {
            TimeScale = Mathf.Clamp(timeScale, 0, 1);
            SaveTimeScaleValue();
        }

        public void SetTimeScaleUnclaimed(float timeScale)
        {
            TimeScale = timeScale;
            SaveTimeScaleValue();
        }

        private void SetTimeScaleViaPause()
        {
            if (isPause)
            {
                TimeScale = 0f;
            }
            else
            {
                TimeScale = timeScaleSaved;
            }
        }

        private void SaveTimeScaleValue()
        {
            timeScaleSaved = TimeScale;
        }
    }
}
