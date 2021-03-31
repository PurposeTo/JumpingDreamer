using System;
using Desdiene.GameDataAsset.DataLoader.ReaderWriterStates.Base;
using Desdiene.GameDataAsset.Model;
using Desdiene.Tools;
using UnityEngine;

namespace Desdiene.GameDataAsset.DataLoader.ReaderWriterStates
{
    internal class DataWasReceivedState<T> : ReaderWriterState<T> where T : GameData
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
