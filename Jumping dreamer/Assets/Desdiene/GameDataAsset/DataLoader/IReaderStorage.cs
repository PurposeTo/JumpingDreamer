using System;
using Desdiene.GameDataAsset.Data;

namespace Desdiene.GameDataAsset.DataLoader
{
    public interface IReaderStorage<T> where T : GameData
    {
        void Read(Action<T> dataCallback);
    }
}
