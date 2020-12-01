using System;
using UnityEngine;

public class PlayerStats : ISetStatsData, IGetAction
{
    public event Action OnNewScoreRecord;

    private PlayerStatsData data = new PlayerStatsData();


    public PlayerStatsData GetDataContainer() => data;
    public void SetData(PlayerStatsData data) => this.data = data;


    public void SetDataWithDefaultValues()
    {
        data = new PlayerStatsData
        {
            MaxCollectedStars = default(int),
            MaxEarnedScore = default(int),
            MaxScoreMultiplierValue = default(int),
            MaxLifeTime = default(int),
            TotalLifeTime = default(int),
        };
    }


    void ISetStatsData.SaveMaxStarsData(SafeInt starsAmount)
    {
        if (starsAmount > data.MaxCollectedStars)
        {
            data.MaxCollectedStars = starsAmount;
        }
    }


    void ISetStatsData.SaveScoreData(SafeInt scoreAmount)
    {
        if (scoreAmount > data.MaxEarnedScore)
        {
            data.MaxEarnedScore = scoreAmount;
            OnNewScoreRecord?.Invoke();
        }
    }


    void ISetStatsData.SaveScoreMultiplierData(SafeInt multiplierValue)
    {
        if (multiplierValue > data.MaxScoreMultiplierValue)
        {
            data.MaxScoreMultiplierValue = multiplierValue;
        }
    }


    void ISetStatsData.SaveLifeTimeData(SafeInt lifeTime)
    {
        data.TotalLifeTime += lifeTime;

        if (lifeTime > data.MaxLifeTime)
        {
            data.MaxLifeTime = lifeTime;
        }
    }


    // Если не хватает какого-либо поля в объекте статов игрока, т.е. значение этого поля равно null, то это значит, что файл изменялся
    public bool HasDataNullValues()
    {
        return
            !(data.MaxCollectedStars.HasValue &&
            data.MaxEarnedScore.HasValue &&
            data.MaxScoreMultiplierValue.HasValue &&
            data.MaxLifeTime.HasValue &&
            data.TotalLifeTime.HasValue);
    }


    public static PlayerStatsData CombineData(PlayerStatsData cloudStatsData, PlayerStatsData localStatsData)
    {
        if (cloudStatsData is null) throw new ArgumentNullException(nameof(cloudStatsData));
        if (localStatsData is null) throw new ArgumentNullException(nameof(localStatsData));

        PlayerStatsData mixedStatsData = new PlayerStatsData
        {
            MaxCollectedStars = Mathf.Max((int)cloudStatsData.MaxCollectedStars, (int)localStatsData.MaxCollectedStars),
            MaxEarnedScore = Mathf.Max((int)cloudStatsData.MaxEarnedScore, (int)localStatsData.MaxEarnedScore),
            MaxLifeTime = Mathf.Max((int)cloudStatsData.MaxLifeTime, (int)localStatsData.MaxLifeTime),
            MaxScoreMultiplierValue = Mathf.Max((int)cloudStatsData.MaxScoreMultiplierValue, (int)localStatsData.MaxScoreMultiplierValue),
            TotalLifeTime = Mathf.Max((int)cloudStatsData.TotalLifeTime, (int)localStatsData.TotalLifeTime)
        };

        return mixedStatsData;
    }
}
