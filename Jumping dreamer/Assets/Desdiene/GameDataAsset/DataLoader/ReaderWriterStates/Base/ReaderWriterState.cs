using System;
using Desdiene.GameDataAsset.Model;
using Desdiene.Tools;

namespace Desdiene.GameDataAsset.DataLoader.ReaderWriterStates.Base
{
    public abstract class ReaderWriterState<T> where T : GameData
    {
        private protected readonly DataStorage<T> dataStorage;


        protected ReaderWriterState(DataStorage<T> dataStorage)
        {
            this.dataStorage = dataStorage;
        }

        public abstract void Read(AtomicReference<ReaderWriterState<T>> state, Action<T> dataCallback);
        public abstract void Write(AtomicReference<ReaderWriterState<T>> state, T data);
    }
}
