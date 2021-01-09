using System;

public abstract class DataStorage : StorageDataReaderWriter
{
    public DataStorage(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour) { }


    private bool isDataReading = false;
    private protected PlayerGameData data;


    public sealed override void ReadAllData(Action<PlayerGameData> callback)
    {
        isDataReading = true;
        Read(data =>
        {
            isDataReading = false;
            callback?.Invoke(data);
        });
    }


    public sealed override void WriteAllData(PlayerGameData data)
    {
        if (!isDataReading) Write(data);
    }


    public abstract void Read(Action<PlayerGameData> callback);
    public abstract void Write(PlayerGameData data);
}
