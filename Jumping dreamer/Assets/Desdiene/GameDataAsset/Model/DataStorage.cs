using System;
using Desdiene.GameDataAsset.DataLoader;
using Desdiene.SuperMonoBehaviourAsset;

namespace Desdiene.GameDataAsset.Model
{
    internal class DataStorage<T> : ReaderWriter<T> where T : GameData
    {
        public string Name { get; } // Имя конкретного хранилища

        public DataStorage(string name, SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour)
        {
            Name = name;
        }


        public override void Read(Action<T> dataCallback)
        {
            throw new NotImplementedException();
        }

        public override void Write(T data)
        {
            throw new NotImplementedException();
        }
    }
}
