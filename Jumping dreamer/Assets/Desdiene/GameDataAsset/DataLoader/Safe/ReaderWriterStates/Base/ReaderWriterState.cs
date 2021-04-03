using System;
using Desdiene.GameDataAsset.Data;
using Desdiene.GameDataAsset.DataLoader.Storage;
using Desdiene.Tools;

namespace Desdiene.GameDataAsset.DataLoader.Safe.ReaderWriterStates.Base
{
    public abstract class ReaderWriterState<T> where T : GameData, new()
    {
        private protected readonly DataStorage<T> dataStorage;


        private protected ReaderWriterState(DataStorage<T> dataStorage)
        {
            if (dataStorage is null) throw new ArgumentNullException(nameof(dataStorage));

            this.dataStorage = dataStorage;
        }

        public abstract void Read(AtomicReference<ReaderWriterState<T>> state, Action<T> dataCallback);
        public abstract void Write(AtomicReference<ReaderWriterState<T>> state, T data);
    }
}
