using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;

public class PlayerStatsData : IGetStatsData, ISetStatsData
{
    public event Action OnNewScoreRecord;

    // Лучшие результаты за все время игры
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? MaxCollectedStars { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? MaxEarnedScore { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? MaxScoreMultiplierValue { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? MaxLifeTime { get; set; }

    // Общие результаты за все время игры
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? TotalLifeTime { get; set; }


    public static PlayerStatsData CreateStatsWithDefaultValues()
    {
        return new PlayerStatsData
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
        if (starsAmount > MaxCollectedStars)
        {
            MaxCollectedStars = starsAmount;
        }
    }


    void ISetStatsData.SaveScoreData(SafeInt scoreAmount)
    {
        if (scoreAmount > MaxEarnedScore)
        {
            MaxEarnedScore = scoreAmount;
            OnNewScoreRecord?.Invoke();
        }
    }


    void ISetStatsData.SaveScoreMultiplierData(SafeInt multiplierValue)
    {
        if (multiplierValue > MaxScoreMultiplierValue)
        {
            MaxScoreMultiplierValue = multiplierValue;
        }
    }


    void ISetStatsData.SaveLifeTimeData(SafeInt lifeTime)
    {
        TotalLifeTime += lifeTime;

        if (lifeTime > MaxLifeTime)
        {
            MaxLifeTime = lifeTime;
        }
    }


    // Если не хватает какого-либо поля в объекте статов игрока, т.е. значение этого поля равно null, то это значит, что файл изменялся
    public bool IsStatsHaveNullValues()
    {

        #region C# 8.0 don't support by Unity
        //PlayerStatsData dataModel = (PlayerStatsData)PlayerStats.Clone();

        //dataModel.MaxCollectedStars ??= default;
        //dataModel.MaxEarnedScore ??= default;
        //dataModel.MaxScoreMultiplierValue ??= default;
        //dataModel.MaxLifeTime ??= default;
        //dataModel.TotalCollectedStars ??= default;
        //dataModel.TotalLifeTime ??= default;

        //if (!dataModel.Equals(PlayerStats))
        //{
        //    haveNullValue = true;
        //    PlayerStats = dataModel;
        //}

        //public override bool Equals(object obj)
        //{
        //    return base.Equals(obj);
        //}


        /// <summary>
        ///  При переопределении метода GetHashCode следует также переопределить Equals и наоборот.Если переопределенный метод Equals возвращает true при проверке на равенство двух объектов, переопределенный метод GetHashCode должен возвращать одно и то же значение для двух объектов.
        /// </summary>
        //public override int GetHashCode()
        //{
        //    return base.GetHashCode();
        //}
        #endregion

        return
            !(MaxCollectedStars.HasValue &&
            MaxEarnedScore.HasValue &&
            MaxScoreMultiplierValue.HasValue &&
            MaxLifeTime.HasValue &&
            TotalLifeTime.HasValue);
    }


    public static PlayerStatsData CombinePlayerStats(PlayerStatsData cloudPlayerStatsData, PlayerStatsData localPlayerStatsData)
    {
        if (cloudPlayerStatsData is null) throw new ArgumentNullException(nameof(cloudPlayerStatsData));
        if (localPlayerStatsData is null) throw new ArgumentNullException(nameof(localPlayerStatsData));

        PlayerStatsData mixedPlayerStatsData = CreateStatsWithDefaultValues();

        mixedPlayerStatsData.MaxCollectedStars = cloudPlayerStatsData.MaxCollectedStars > localPlayerStatsData.MaxCollectedStars ? cloudPlayerStatsData.MaxCollectedStars : localPlayerStatsData.MaxCollectedStars;

        mixedPlayerStatsData.MaxEarnedScore = cloudPlayerStatsData.MaxEarnedScore > localPlayerStatsData.MaxEarnedScore ? cloudPlayerStatsData.MaxEarnedScore : localPlayerStatsData.MaxEarnedScore;

        mixedPlayerStatsData.MaxLifeTime = cloudPlayerStatsData.MaxLifeTime > localPlayerStatsData.MaxLifeTime ? cloudPlayerStatsData.MaxLifeTime : localPlayerStatsData.MaxLifeTime;

        mixedPlayerStatsData.MaxScoreMultiplierValue = cloudPlayerStatsData.MaxScoreMultiplierValue > localPlayerStatsData.MaxScoreMultiplierValue ? cloudPlayerStatsData.MaxScoreMultiplierValue : localPlayerStatsData.MaxScoreMultiplierValue;

        mixedPlayerStatsData.TotalLifeTime = cloudPlayerStatsData.TotalLifeTime > localPlayerStatsData.TotalLifeTime ? cloudPlayerStatsData.TotalLifeTime : localPlayerStatsData.TotalLifeTime;

        mixedPlayerStatsData.MaxCollectedStars = cloudPlayerStatsData.MaxCollectedStars > localPlayerStatsData.MaxCollectedStars ? cloudPlayerStatsData.MaxCollectedStars : localPlayerStatsData.MaxCollectedStars;

        return mixedPlayerStatsData;
    }


    #region Моя реализация Equals
    //public override bool Equals(object obj)
    //{
    //    return obj is PlayerStatsData && base.Equals(obj);
    //}
    #endregion


    // TODO: Работает?
    public override bool Equals(object obj)
    {
        return obj is PlayerStatsData data &&
               EqualityComparer<SafeInt?>.Default.Equals(MaxCollectedStars, data.MaxCollectedStars) &&
               EqualityComparer<SafeInt?>.Default.Equals(MaxEarnedScore, data.MaxEarnedScore) &&
               EqualityComparer<SafeInt?>.Default.Equals(MaxScoreMultiplierValue, data.MaxScoreMultiplierValue) &&
               EqualityComparer<SafeInt?>.Default.Equals(MaxLifeTime, data.MaxLifeTime) &&
               EqualityComparer<SafeInt?>.Default.Equals(TotalLifeTime, data.TotalLifeTime);
    }


    // TODO: Работает?
    public override int GetHashCode()
    {
        int hashCode = 502919464;
        hashCode = hashCode * -1521134295 + MaxCollectedStars.GetHashCode();
        hashCode = hashCode * -1521134295 + MaxEarnedScore.GetHashCode();
        hashCode = hashCode * -1521134295 + MaxScoreMultiplierValue.GetHashCode();
        hashCode = hashCode * -1521134295 + MaxLifeTime.GetHashCode();
        hashCode = hashCode * -1521134295 + TotalLifeTime.GetHashCode();

        return hashCode;
    }
}
