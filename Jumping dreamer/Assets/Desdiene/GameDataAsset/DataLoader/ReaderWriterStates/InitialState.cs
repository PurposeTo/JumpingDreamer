using System;
using Desdiene.GameDataAsset.DataLoader.ReaderWriterStates.Base;
using Desdiene.GameDataAsset.Model;
using Desdiene.Tools;
using UnityEngine;

namespace Desdiene.GameDataAsset.DataLoader.ReaderWriterStates
{
    internal class InitialState<T> : ReaderWriterState<T> where T : GameData
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
