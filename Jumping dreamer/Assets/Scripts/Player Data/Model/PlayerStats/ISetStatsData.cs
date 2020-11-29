public interface ISetStatsData
{
    void SaveMaxStarsData(SafeInt starsAmount);
    void SaveScoreData(SafeInt score);
    void SaveScoreMultiplierData(SafeInt multiplier);
    void SaveLifeTimeData(SafeInt lifeTime);
}
