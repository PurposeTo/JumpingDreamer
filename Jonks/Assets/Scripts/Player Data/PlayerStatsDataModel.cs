using System;

[Serializable]
public class PlayerStatsDataModel : ICloneable
{
    // Лучшие результаты за все время игры
    public int? MaxCollectedStars { get; set; }
    public int? MaxEarnedScore { get; set; }
    public int? MaxScoreMultiplierValue { get; set; }
    public int? MaxLifeTime { get; set; }
    //public float? maxJumpHeight; // json хранит double

    // Общие результаты за все время игры
    public int? TotalCollectedStars { get; set; }
    public int? TotalLifeTime { get; set; }


    public static PlayerStatsDataModel CreateModelWithDefaultValues()
    {
        return new PlayerStatsDataModel
        {
            MaxCollectedStars = default(int),
            MaxEarnedScore = default(int),
            MaxScoreMultiplierValue = default(int),
            MaxLifeTime = default(int),
            TotalCollectedStars = default(int),
            TotalLifeTime = default(int),
        };
    }


    // Если не хватает какого-либо поля в файле, т.е. его значение равно null, то это значит, что файл изменялся
    public bool TrySetDefaultValues()
    {
        bool haveNullValue = false;

        #region C# 8.0 don't support by Unity
        //PlayerStatsDataModel dataModel = (PlayerStatsDataModel)PlayerStatsData.Clone();

        //dataModel.MaxCollectedStars ??= default;
        //dataModel.MaxEarnedScore ??= default;
        //dataModel.MaxScoreMultiplierValue ??= default;
        //dataModel.MaxLifeTime ??= default;
        //dataModel.TotalCollectedStars ??= default;
        //dataModel.TotalLifeTime ??= default;

        //if (dataModel != PlayerStatsData)
        //{
        //    haveNullValue = true;
        //    PlayerStatsData = dataModel;
        //}
        #endregion

        // Установить все null values И ТОЛЬКО ИХ! в default для того, чтобы можно было продолжить работу с ними
        if (!MaxCollectedStars.HasValue)
        {
            MaxCollectedStars = default(int);
            haveNullValue = true;
        }
        if (!MaxEarnedScore.HasValue)
        {
            MaxEarnedScore = default(int);
            haveNullValue = true;
        }
        if (!MaxScoreMultiplierValue.HasValue)
        {
            MaxScoreMultiplierValue = default(int);
            haveNullValue = true;
        }
        if (!MaxLifeTime.HasValue)
        {
            MaxLifeTime = default(int);
            haveNullValue = true;
        }
        if (!TotalCollectedStars.HasValue)
        {
            TotalCollectedStars = default(int);
            haveNullValue = true;
        }
        if (!TotalLifeTime.HasValue)
        {
            TotalLifeTime = default(int);
            haveNullValue = true;
        }

        return haveNullValue;
    }


    public object Clone()
    {
        return MemberwiseClone();
    }

}
