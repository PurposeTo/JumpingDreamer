using Desdiene.Singleton;

namespace Desdiene.TimeControl
{
    public class GlobalPause : LazySingleton<GlobalPause>
    {
        private readonly GlobalTimeScaler timeScaler = new GlobalTimeScaler();

        //Поля должны соответствовать логике "pause": если true, то пауза.

        private bool isDeathPause;
        private bool isPlayerPause;
        private bool isSceneLoading;
        private bool isInternetConnectionWaiting;

        public bool IsPause => timeScaler.IsPause;

        public void SetDeathPause(bool pause)
        {
            isDeathPause = pause;
            timeScaler.SetPause(GetTotalPause());
        }

        public void SetPlayerPause(bool pause)
        {
            isPlayerPause = pause;
            timeScaler.SetPause(GetTotalPause());
        }

        public void SetSceneLoading(bool pause)
        {
            isSceneLoading = pause;
            timeScaler.SetPause(GetTotalPause());
        }

        public void SetInternetConnectionWaiting(bool pause)
        {
            isInternetConnectionWaiting = pause;
            timeScaler.SetPause(GetTotalPause());
        }

        private bool GetTotalPause()
        {
            return isDeathPause ||
                isPlayerPause ||
                isSceneLoading ||
                isInternetConnectionWaiting;
        }
    }
}
