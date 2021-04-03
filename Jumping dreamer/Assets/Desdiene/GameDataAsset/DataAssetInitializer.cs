using System.Linq;
using Desdiene.GameDataAsset.Data;
using Desdiene.GameDataAsset.DataLoader;
using Desdiene.GameDataAsset.DataLoader.Safe;
using Desdiene.GameDataAsset.DataLoader.Storage;
using Desdiene.GameDataAsset.DataSynchronizer;
using Desdiene.SuperMonoBehaviourAsset;

namespace Desdiene.GameDataAsset
{
    public class DataAssetInitializer<TData, TGetter, TSetter, TChangingNotifier>
        where TGetter : Data.IDataGetter
        where TData : GameData, TGetter, new()
        where TSetter : DataSetter<TData>, Data.IDataSetter, new()
        where TChangingNotifier : Data.IDataChangingNotifier
    {
        public readonly Model.DataModel<TData, TGetter, TSetter, TChangingNotifier> dataModel;
        public readonly Synchronizer<TData> synchronizer;

        public DataAssetInitializer(SuperMonoBehaviour superMonoBehaviour, params DataStorage<TData>[] storages)
        {
            dataModel = new Model.DataModel<TData, TGetter, TSetter, TChangingNotifier>();

            ReaderWriter<TData>[] safeReaderWriters = storages
                .Select(storage => new SafeReaderWriter<TData>(superMonoBehaviour, storage))
                .ToArray();

            ReaderWriter<TData> readerWritersContainer = new DataReaderWritersContainer<TData>(superMonoBehaviour, safeReaderWriters);
            synchronizer = new Synchronizer<TData>(superMonoBehaviour, dataModel, readerWritersContainer);
        }
    }
}
