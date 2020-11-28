using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using Newtonsoft.Json;

public class PlayerStatsData
{
    public event Action OnNewScoreRecord;

    // Лучшие результаты за все время игры
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? MaxCollectedStars { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? MaxEarnedScore { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? MaxScoreMultiplierValue { get; set; }
    [JsonConverter(typeof(SafeIntConverter))] public SafeInt? MaxLifeTime { get; set; }
    //public float MaxJumpHeight; // json хранит double

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


    public void SaveMaxStarsData(SafeInt starsAmount)
    {
        if (starsAmount > PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.MaxCollectedStars)
        {
            PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.MaxCollectedStars = starsAmount;
        }
    }


    public void SaveScoreData(SafeInt scoreAmount)
    {
        if (scoreAmount > PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.MaxEarnedScore)
        {
            PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.MaxEarnedScore = scoreAmount;
            OnNewScoreRecord?.Invoke();
        }
    }


    public void SaveScoreMultiplierData(SafeInt multiplierValue)
    {
        if (multiplierValue > PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.MaxScoreMultiplierValue)
        {
            PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.MaxScoreMultiplierValue = multiplierValue;
        }
    }


    public void SaveLifeTimeData(SafeInt lifeTime)
    {
        PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.TotalLifeTime += lifeTime;

        if (lifeTime > PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.MaxLifeTime)
        {
            PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.MaxLifeTime = lifeTime;
        }
    }


    [Obsolete]
    public void SaveJumpHeightData(float jumpHeight)
    {
        //if (jumpHeight > PlayerStatsData.PlayerStats.MaxJumpHeight)
        //{
        //    PlayerStatsData.PlayerStats.MaxJumpHeight = jumpHeight;
        //}
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


    public static PlayerStatsData MixPlayerStats(PlayerStatsData cloudPlayerStatsData, PlayerStatsData localPlayerStatsData)
    {
        if (cloudPlayerStatsData == null)
        {
            return localPlayerStatsData;
        }

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
