using System;

public class DataSynchronizer : IDataInModelUpdater, IDataInStorageUpdater, IDataReseter, ISynchronizerNotifier
{
    private readonly IModelInteraction modelInteraction;
    private readonly StorageDataReaderWriter readerWriter;


    public DataSynchronizer(IModelInteraction modelInteraction, StorageDataReaderWriter readerWriter)
    {
        this.modelInteraction = modelInteraction;
        this.readerWriter = readerWriter;
    }


    private PlayerGameData data;
    private readonly DataCombiner dataCombiner = new DataCombiner();


    void IDataInModelUpdater.UpdateModel()
    {
        readerWriter.ReadAllData(receivedData =>
        {
            CombineDataFromStorages(receivedData);
            CombineModelAndStorageDatas(data);
        });
    }


    void IDataInStorageUpdater.UpdateStorage()
    {
        readerWriter.WriteAllData(modelInteraction.GetData());
    }


    void IDataReseter.Reset()
    {
        modelInteraction.SetData(PlayerGameData.CreateDataWithDefaultValues());
    }


    private void CombineDataFromStorages(PlayerGameData storageData)
    {
        if (storageData == null) throw new ArgumentNullException(nameof(storageData));

        // Выполняется при первом получении данных
        if (data == null)
        {
            data = storageData;
            return;
        }

        // Выполняется при последующем получении данных из другого хранилища
        if (data.Id != storageData.Id)
        {
            // TODO: Вывод диалогового окна
        }
        else if (!data.Equals(storageData))
        {
            data = dataCombiner.Combine(data, storageData);
        }
    }


    private void CombineModelAndStorageDatas(PlayerGameData storageData)
    {
        throw new NotImplementedException();
    }
}
