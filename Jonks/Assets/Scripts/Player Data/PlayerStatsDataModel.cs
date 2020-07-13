using System;

namespace Assets.Scripts.Player.DataModel
{
    [Serializable]
    public class PlayerStatsDataModel
    {
        // Лучшие результаты за все время игры
        public int maxCollectedStarsAmount;
        public int maxEarnedPointsAmount;
        public int maxPointsMultiplierValue;
        public int maxLifeTime;
        //public float maxJumpHeight; // json хранит double

        // Общие результаты за все время игры
        public int totalCollectedStarsAmount;
        public int totalLifeTime;
    }
}
