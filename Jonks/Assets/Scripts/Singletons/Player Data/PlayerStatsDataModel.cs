using System;

namespace Assets.Scripts.Player.DataModel
{
    [Serializable]
    public class PlayerStatsDataModel : Singleton<PlayerStatsDataModel>
    {
        // Лучшие результаты за все время игры
        public int maxCollectedCoinsNumber;
        public int maxEarnedPointsNumber;
        public int maxPointsMultiplierValue;
        public float maxLifeTime;
        public float maxJumpHeight;

        // Общие результаты за все время игры
        public int totalCollectedCoinsNumber;
        public TimeSpan totalGameTime;
    }
}
