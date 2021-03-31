using System;
using Desdiene.Container;
using Desdiene.GameDataAsset.Model;
using Desdiene.SuperMonoBehaviourAsset;

namespace Desdiene.GameDataAsset.DataLoader
{
    public abstract class ReaderWriter<T> : SuperMonoBehaviourContainer where T : GameData
    {
        protected ReaderWriter(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour) { }


        public abstract void Read(Action<T> dataCallback);
        public abstract void Write(T data);
    }
}
