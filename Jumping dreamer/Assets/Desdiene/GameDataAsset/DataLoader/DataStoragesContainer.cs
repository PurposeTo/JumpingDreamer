using System;
using Desdiene.GameDataAsset.Model;
using Desdiene.SuperMonoBehaviourAsset;

namespace Desdiene.GameDataAsset.DataLoader
{
    internal class DataStoragesContainer<T> : ReaderWriter<T> where T : GameData
    {
        private readonly ReaderWriter<T>[] storages;

        public DataStoragesContainer(SuperMonoBehaviour superMonoBehaviour, params ReaderWriter<T>[] storages)
            : base(superMonoBehaviour)
        {
            this.storages = storages;
        }

        public override void Read(Action<T> data)
        {
            Array.ForEach(storages, storage => storage.Read(data));
        }

        public override void Write(T data)
        {
            Array.ForEach(storages, storage => storage.Write(data));
        }
    }
}
