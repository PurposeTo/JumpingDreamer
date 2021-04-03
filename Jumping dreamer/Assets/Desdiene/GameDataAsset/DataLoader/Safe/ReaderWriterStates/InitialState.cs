using System;
using Desdiene.GameDataAsset.Data;
using Desdiene.GameDataAsset.DataLoader.Safe.ReaderWriterStates.Base;
using Desdiene.GameDataAsset.DataLoader.Storage;
using Desdiene.Tools;
using UnityEngine;

namespace Desdiene.GameDataAsset.DataLoader.Safe.ReaderWriterStates
{
    internal class InitialState<T> : ReaderWriterState<T> where T : GameData, new()
    {
        public InitialState(DataStorage<T> dataStorage) : base(dataStorage) { }

        public override void Read(AtomicReference<ReaderWriterState<T>> state, Action<T> dataCallback)
        {
            dataStorage.Read(data =>
            {
                dataCallback?.Invoke(data);
                state.Set(new DataWasReceivedState<T>(dataStorage));
            });


        }

        public override void Write(AtomicReference<ReaderWriterState<T>> state, T data)
        {
            Debug.Log($"Данные с [{dataStorage.Name}] еще не были получены. " +
                $"Запись невозможна! Иначе данное действие перезапишет еще не полученные данные.");
        }
    }
}
