using System;
using Desdiene.Super_monoBehaviour;

public abstract class StorageDataReaderWriter : SuperMonoBehaviourContainer
{
    protected StorageDataReaderWriter(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour) { }


    public abstract void ReadAllData(Action<PlayerGameData> dataCallback);
    public abstract void WriteAllData(PlayerGameData data);
}
