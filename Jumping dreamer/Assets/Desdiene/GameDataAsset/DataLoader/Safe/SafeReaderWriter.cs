using System;
using Desdiene.GameDataAsset.Data;
using Desdiene.GameDataAsset.DataLoader.Safe.ReaderWriterStates;
using Desdiene.GameDataAsset.DataLoader.Safe.ReaderWriterStates.Base;
using Desdiene.GameDataAsset.DataLoader.Storage;
using Desdiene.SuperMonoBehaviourAsset;
using Desdiene.Tools;

namespace Desdiene.GameDataAsset.DataLoader.Safe
{
    //todo Не нужен superMonoBehaviour! Заменить наследование от класса на реализацию интерфейса
    internal class SafeReaderWriter<T> : ReaderWriter<T> where T : GameData, new()
    {
        private readonly AtomicReference<ReaderWriterState<T>> readerWriterState;

        public SafeReaderWriter(SuperMonoBehaviour superMonoBehaviour, DataStorage<T> dataStorage) : base(superMonoBehaviour)
        {
            if (dataStorage is null) throw new ArgumentNullException(nameof(dataStorage));

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
