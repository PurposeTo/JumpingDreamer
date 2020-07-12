using System;

namespace Assets.Scripts.Player.DataModel
{
    [Serializable]
    public class PlayerStatsDataModel
    {
        // Лучшие результаты за все время игры
        public int maxCollectedCoinsAmount;
        public int maxEarnedPointsAmount;
        public int maxPointsMultiplierValue;
        public float maxLifeTime;
        //public float maxJumpHeight;

        // Общие результаты за все время игры
        public int totalCollectedCoinsAmount;
        public float totalLifeTime;
    }
}
