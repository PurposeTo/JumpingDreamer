using System;
using Boo.Lang;
using Newtonsoft.Json;

public class PlayerStatsData
{
    public event EventHandler OnNewScoreRecord;

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
        if (starsAmount > PlayerDataStorageSafe.Instance. PlayerDataModel.PlayerStats.MaxCollectedStars)
        {
            PlayerDataStorageSafe.Instance.PlayerDataModel.PlayerStats.MaxCollectedStars = starsAmount;
        }
    }


    public void SaveScoreData(SafeInt scoreAmount)
    {
        if (scoreAmount > PlayerDataStorageSafe.Instance.PlayerDataModel.PlayerStats.MaxEarnedScore)
        {
            PlayerDataStorageSafe.Instance.PlayerDataModel.PlayerStats.MaxEarnedScore = scoreAmount;
            OnNewScoreRecord?.Invoke(this, null);
        }
    }


    public void SaveScoreMultiplierData(SafeInt multiplierValue)
    {
        if (multiplierValue > PlayerDataStorageSafe.Instance.PlayerDataModel.PlayerStats.MaxScoreMultiplierValue)
        {
            PlayerDataStorageSafe.Instance.PlayerDataModel.PlayerStats.MaxScoreMultiplierValue = multiplierValue;
        }
    }


    public void SaveLifeTimeData(SafeInt lifeTime)
    {
        PlayerDataStorageSafe.Instance.PlayerDataModel.PlayerStats.TotalLifeTime += lifeTime;

        if (lifeTime > PlayerDataStorageSafe.Instance.PlayerDataModel.PlayerStats.MaxLifeTime)
        {
            PlayerDataStorageSafe.Instance.PlayerDataModel.PlayerStats.MaxLifeTime = lifeTime;
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
        //bool haveNullValue = false;

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

        #region set default values only for fields, which have null value
        //// Установить все null values И ТОЛЬКО ИХ! в default для того, чтобы можно было продолжить работу с ними
        //if (!MaxCollectedStars.HasValue)
        //{
        //    MaxCollectedStars = default(int);
        //    haveNullValue = true;
        //}
        //if (!MaxEarnedScore.HasValue)
        //{
        //    MaxEarnedScore = default(int);
        //    haveNullValue = true;
        //}
        //if (!MaxScoreMultiplierValue.HasValue)
        //{
        //    MaxScoreMultiplierValue = default(int);
        //    haveNullValue = true;
        //}
        //if (!MaxLifeTime.HasValue)
        //{
        //    MaxLifeTime = default(int);
        //    haveNullValue = true;
        //}
        //if (!TotalCollectedStars.HasValue)
        //{
        //    TotalCollectedStars = default(int);
        //    haveNullValue = true;
        //}
        //if (!TotalLifeTime.HasValue)
        //{
        //    TotalLifeTime = default(int);
        //    haveNullValue = true;
        //}
        #endregion

        //return haveNullValue;

        return
            !(MaxCollectedStars.HasValue &&
            MaxEarnedScore.HasValue &&
            MaxScoreMultiplierValue.HasValue &&
            MaxLifeTime.HasValue &&
            TotalLifeTime.HasValue);
    }


    public static PlayerStatsData MixPlayerStats(PlayerStatsData cloudPlayerStatsData, PlayerStatsData localPlayerStatsData)
    {
        if (cloudPlayerStatsData is null)
        {
            return localPlayerStatsData;
        }

        PlayerStatsData mixedPlayerStatsData = null;

        // TODO: mix stats

        return mixedPlayerStatsData;
    }


    // Работает?
    public override bool Equals(object obj)
    {
        return base.Equals(obj);
    }


    // Работает?
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }
}
