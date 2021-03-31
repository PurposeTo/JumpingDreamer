using System;
using Desdiene.GameDataAsset.DataLoader.ReaderWriterStates;
using Desdiene.GameDataAsset.DataLoader.ReaderWriterStates.Base;
using Desdiene.GameDataAsset.Model;
using Desdiene.SuperMonoBehaviourAsset;
using Desdiene.Tools;

namespace Desdiene.GameDataAsset.DataLoader
{
    internal class SafeReaderWriter<T> : ReaderWriter<T> where T : GameData
    {
        private readonly AtomicReference<ReaderWriterState<T>> readerWriterState;

        public SafeReaderWriter(SuperMonoBehaviour superMonoBehaviour, DataStorage<T> dataStorage) : base(superMonoBehaviour)
        {
            readerWriterState = new AtomicReference<ReaderWriterState<T>>(new InitialState<T>(dataStorage));
        }

        public override void Read(Action<T> dataCallback)
        {
            readerWriterState.Get().Read(readerWriterState, dataCallback);
        }

        public override void Write(T data)
        {
            readerWriterState.Get().Write(readerWriterState, data);
        }

    }
}
