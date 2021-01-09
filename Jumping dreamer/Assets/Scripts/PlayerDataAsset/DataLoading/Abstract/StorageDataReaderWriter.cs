using System;

public abstract class StorageDataReaderWriter : SuperMonoBehaviourContainer
{
    protected StorageDataReaderWriter(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour) { }


    public abstract void ReadAllData(Action<PlayerGameData> callback);
    public abstract void WriteAllData(PlayerGameData data);
}
