using System;
using Desdiene.Container;
using Desdiene.GameDataAsset.Data;
using Desdiene.SuperMonoBehaviourAsset;

namespace Desdiene.GameDataAsset.DataLoader
{
    public abstract class ReaderWriter<T> : 
        SuperMonoBehaviourContainer, 
        IReaderStorage<T>, 
        IWriterStorage<T> 
        where T : GameData
    {
        protected ReaderWriter(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour) { }


        public abstract void Read(Action<T> dataCallback);
        public abstract void Write(T data);
    }
}
