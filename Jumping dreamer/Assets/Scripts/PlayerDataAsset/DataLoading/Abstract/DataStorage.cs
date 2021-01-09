using System;

public abstract class DataStorage : StorageDataReaderWriter
{
    public DataStorage(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour) { }


    private bool isDataReading = false;
    private protected Data data;


    public sealed override void ReadAllData(Action<Data> callback)
    {
        isDataReading = true;
        Read(data =>
        {
            isDataReading = false;
            callback?.Invoke(data);
        });
    }


    public sealed override void WriteAllData(Data data)
    {
        if (!isDataReading) Write(data);
    }


    public abstract void Read(Action<Data> callback);
    public abstract void Write(Data data);
}
