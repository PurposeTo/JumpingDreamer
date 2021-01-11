using System;
using System.Collections.Generic;

public class DataLoader : StorageDataReaderWriter
{
    private readonly StorageDataReaderWriter[] storages;


    public DataLoader(SuperMonoBehaviour superMonoBehaviour, params StorageDataReaderWriter[] storages) : base(superMonoBehaviour)
    {
        this.storages = storages;
    }


    public override void ReadAllData(Action<PlayerGameData> dataCallback)
    {
        Array.ForEach(storages, storage => storage.ReadAllData(dataCallback));
    }


    public override void WriteAllData(PlayerGameData data)
    {
        Array.ForEach(storages, storage => storage.WriteAllData(data));
    }
}
