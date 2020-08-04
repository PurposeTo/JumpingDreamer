using System;

[Serializable]
public class PlayerStatsDataModel
{
    // Лучшие результаты за все время игры
    public int MaxCollectedStars { get; set; }
    public int MaxEarnedScore { get; set; }
    public int MaxScoreMultiplierValue { get; set; }
    public int MaxLifeTime { get; set; }
    //public float maxJumpHeight; // json хранит double

    // Общие результаты за все время игры
    public int TotalCollectedStars { get; set; }
    public int TotalLifeTime { get; set; }


    public static PlayerStatsDataModel CreateModelWithDefaultValues()
    {
        return new PlayerStatsDataModel
        {
            MaxCollectedStars = default,
            MaxEarnedScore = default,
            MaxScoreMultiplierValue = default,
            MaxLifeTime = default,
            TotalCollectedStars = default,
            TotalLifeTime = default,
        };
    }


    //// Если не хватает какого-либо поля в файле, т.е. его значение равно null, то это значит, что файл изменялся
    //public bool TrySetDefaultValues()
    //{
    //    bool haveNullValue = false;

    //    #region C# 8.0 don't support by Unity
    //    //PlayerStatsDataModel dataModel = (PlayerStatsDataModel)PlayerStatsData.Clone();

    //    //dataModel.MaxCollectedStars ??= default;
    //    //dataModel.MaxEarnedScore ??= default;
    //    //dataModel.MaxScoreMultiplierValue ??= default;
    //    //dataModel.MaxLifeTime ??= default;
    //    //dataModel.TotalCollectedStars ??= default;
    //    //dataModel.TotalLifeTime ??= default;

    //    //if (!dataModel.Equals(PlayerStatsData))
    //    //{
    //    //    haveNullValue = true;
    //    //    PlayerStatsData = dataModel;
    //    //}

    //    //public override bool Equals(object obj)
    //    //{
    //    //    return base.Equals(obj);
    //    //}


    //    /// <summary>
    //    ///  При переопределении метода GetHashCode следует также переопределить Equals и наоборот.Если переопределенный метод Equals возвращает true при проверке на равенство двух объектов, переопределенный метод GetHashCode должен возвращать одно и то же значение для двух объектов.
    //    /// </summary>
    //    //public override int GetHashCode()
    //    //{
    //    //    return base.GetHashCode();
    //    //}
    //    #endregion

    //    // Установить все null values И ТОЛЬКО ИХ! в default для того, чтобы можно было продолжить работу с ними
    //    if (!MaxCollectedStars.HasValue)
    //    {
    //        MaxCollectedStars = default(int);
    //        haveNullValue = true;
    //    }
    //    if (!MaxEarnedScore.HasValue)
    //    {
    //        MaxEarnedScore = default(int);
    //        haveNullValue = true;
    //    }
    //    if (!MaxScoreMultiplierValue.HasValue)
    //    {
    //        MaxScoreMultiplierValue = default(int);
    //        haveNullValue = true;
    //    }
    //    if (!MaxLifeTime.HasValue)
    //    {
    //        MaxLifeTime = default(int);
    //        haveNullValue = true;
    //    }
    //    if (!TotalCollectedStars.HasValue)
    //    {
    //        TotalCollectedStars = default(int);
    //        haveNullValue = true;
    //    }
    //    if (!TotalLifeTime.HasValue)
    //    {
    //        TotalLifeTime = default(int);
    //        haveNullValue = true;
    //    }

    //    return haveNullValue;
    //}
}
