using System;
using Desdiene.SuperMonoBehaviourAsset;
using UnityEngine;

public abstract class DataStorageOld : StorageDataReaderWriter
{
    public DataStorageOld(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour) { }


    private protected PlayerGameData data;

    private bool isDataReading = false;
    private bool isDataHasAlreadyReaded = false;


    public sealed override void ReadAllData(Action<PlayerGameData> dataCallback)
    {
        if (isDataHasAlreadyReaded) Debug.LogWarning($"Данные с {this.GetType()} уже были загружены!");
        else
        {
            isDataReading = true;

            ReadFromStorage(data =>
            {
                isDataReading = false;
                dataCallback?.Invoke(data);
            });

            isDataHasAlreadyReaded = true;
        }
    }


    public sealed override void WriteAllData(PlayerGameData data)
    {
        if (!isDataReading)
        {
            if (!new Validator().HasJsonNullValues(DataConverter.ToJson(data, out bool _1, out Exception _2)))
            {
                WriteToStorage(data);
            }
        }
    }


    private protected abstract void ReadFromStorage(Action<PlayerGameData> dataCallback);
    private protected abstract void WriteToStorage(PlayerGameData data);
}
