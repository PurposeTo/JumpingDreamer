public interface IStatsSetter
{
    void SaveRecordStarsData(SafeInt starsAmount);
    void SaveScoreData(SafeInt score);
    void SaveScoreMultiplierData(SafeInt multiplier);
    void SaveLifeTimeData(SafeInt lifeTime);
}
