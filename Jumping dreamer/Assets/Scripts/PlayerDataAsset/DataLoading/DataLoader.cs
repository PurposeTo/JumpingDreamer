using System;
using System.Collections.Generic;

public class DataLoader : StorageDataReaderWriter
{
    private readonly List<StorageDataReaderWriter> storages;


    public DataLoader(SuperMonoBehaviour superMonoBehaviour, List<StorageDataReaderWriter> storages) : base(superMonoBehaviour)
    {
        this.storages = storages;
    }


    public override void ReadAllData(Action<Data> callback)
    {
        storages.ForEach(storage => storage.ReadAllData(callback));
    }


    public override void WriteAllData(Data data)
    {
        storages.ForEach(storage => storage.WriteAllData(data));
    }
}
