using System.Linq;
using Desdiene.GameDataAsset.DataLoader;
using Desdiene.GameDataAsset.Model;
using Desdiene.SuperMonoBehaviourAsset;

namespace Desdiene.GameDataAsset
{
    internal class DataStoragesReaderWriterFactory
    {
        static public ReaderWriter<T> GetReaderWriter<T>(SuperMonoBehaviour superMonoBehaviour, params DataStorage<T>[] storages)
            where T : GameData
        {
            ReaderWriter<T>[] safeReaderWriter = storages
                .Select(storage => new SafeReaderWriter<T>(superMonoBehaviour, storage)).ToArray();

            return new DataStoragesContainer<T>(superMonoBehaviour, safeReaderWriter);
        }

    }
}
