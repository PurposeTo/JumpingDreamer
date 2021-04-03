using System;
using Desdiene.GameDataAsset.Data;
using Desdiene.GameDataAsset.DataLoader.Safe.ReaderWriterStates.Base;
using Desdiene.GameDataAsset.DataLoader.Storage;
using Desdiene.Tools;
using UnityEngine;

namespace Desdiene.GameDataAsset.DataLoader.Safe.ReaderWriterStates
{
    internal class DataWasReceivedState<T> : ReaderWriterState<T> where T : GameData, new()
    {
        public DataWasReceivedState(DataStorage<T> dataStorage) : base(dataStorage) { }


        public override void Read(AtomicReference<ReaderWriterState<T>> state, Action<T> dataCallback)
        {
            Debug.Log($"Данные с [{dataStorage.Name}] уже были получены!");
        }

        public override void Write(AtomicReference<ReaderWriterState<T>> state, T data)
        {
            dataStorage.Write(data);
        }
    }
}
