using System;

public abstract class ReadingAndWritingDataToStorage
{
    public abstract void WriteAllData();
    public abstract void ReadAllData(Action<PlayerGameData> callback);
}
