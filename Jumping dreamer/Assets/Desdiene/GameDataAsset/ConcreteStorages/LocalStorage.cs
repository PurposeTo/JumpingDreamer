using System;
using Desdiene.GameDataAsset.Data;
using Desdiene.GameDataAsset.DataLoader.Storage;
using Desdiene.SuperMonoBehaviourAsset;
using Newtonsoft.Json;

namespace Desdiene.GameDataAsset.ConcreteStorages
{
    public class LocalStorage<T> : DataStorage<T>
         where T : GameData, new()
    {
        public LocalStorage(SuperMonoBehaviour superMonoBehaviour, JsonSerializerSettings serializerSettings)
            : base(superMonoBehaviour, "Локальное хранилище", serializerSettings) { }


        protected override void Read(Action<string> dataCallback)
        {
            throw new NotImplementedException();
        }

        protected override void Write(string jsonData)
        {
            throw new NotImplementedException();
        }
    }
}
