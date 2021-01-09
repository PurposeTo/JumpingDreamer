public interface IStatsSetter
{
    void SaveRecordStars(SafeInt starsAmount);
    void SaveRecordScore(SafeInt score);
    void SaveRecordScoreMultiplier(SafeInt multiplier);
    void SaveRecordLifeTime(SafeInt lifeTime);
}
