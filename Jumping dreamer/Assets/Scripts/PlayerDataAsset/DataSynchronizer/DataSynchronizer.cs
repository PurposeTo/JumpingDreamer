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


    private Data data;


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
        modelInteraction.SetData(Data.CreateDataWithDefaultValues());
    }


    private void CombineDataFromStorages(Data storageData)
    {
        if (storageData == null) throw new ArgumentNullException(nameof(storageData));

        if (data == null)
        {
            data = storageData;
            return;
        }

        if (data.Id != storageData.Id)
        {
            // Вывод диалогового окна
        }
        else if (!data.Equals(storageData))
        {
            // Объединение (mixing)
        }
    }


    private void CombineModelAndStorageDatas(Data storagesData)
    {

    }
}
