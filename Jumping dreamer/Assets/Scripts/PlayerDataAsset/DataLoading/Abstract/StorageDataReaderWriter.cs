using System;

public abstract class StorageDataReaderWriter : SuperMonoBehaviourContainer
{
    protected StorageDataReaderWriter(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour) { }


    public abstract void WriteAllData(Data data);
    public abstract void ReadAllData(Action<Data> callback);
}
