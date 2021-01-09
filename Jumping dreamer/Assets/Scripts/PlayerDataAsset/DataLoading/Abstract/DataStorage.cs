using System;
using UnityEngine;

public abstract class DataStorage : StorageDataReaderWriter
{
    public DataStorage(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour) { }


    private protected PlayerGameData data;

    private bool isDataReading = false;
    private bool isDataHasAlreadyReaded = false;


    public sealed override void ReadAllData(Action<PlayerGameData> callback)
    {
        if (isDataHasAlreadyReaded) Debug.LogWarning($"Данные с {this.GetType()} уже были загружены!");
        else
        {
            isDataReading = true;

            ReadFromStorage(data =>
            {
                isDataReading = false;
                callback?.Invoke(data);
            });

            isDataHasAlreadyReaded = true;
        }
    }


    public sealed override void WriteAllData(PlayerGameData data)
    {
        if (!isDataReading) WriteToStorage(data);
    }


    private protected abstract void ReadFromStorage(Action<PlayerGameData> callback);
    private protected abstract void WriteToStorage(PlayerGameData data);
}
