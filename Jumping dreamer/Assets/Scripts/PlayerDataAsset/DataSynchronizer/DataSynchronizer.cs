using System;
using UnityEngine;

public class DataSynchronizer : IDataFromStorageToModelUpdater, IDataFromModelToStorageUpdater, IDataReseter, ISynchronizerNotifier
{
    private readonly IDataHandlerInteraction dataHandlerInteraction;
    private readonly IDataGetter dataGetter;
    private readonly StorageDataReaderWriter readerWriter;

    public DataSynchronizer(IDataHandlerInteraction dataHandlerInteraction, IDataGetter dataGetter, StorageDataReaderWriter readerWriter)
    {
        this.dataGetter = dataGetter ?? throw new ArgumentNullException(nameof(dataGetter));
        this.dataHandlerInteraction = dataHandlerInteraction ?? throw new ArgumentNullException(nameof(dataHandlerInteraction));
        this.readerWriter = readerWriter ?? throw new ArgumentNullException(nameof(readerWriter));
    }


    public event Action OnResetPlayerData;
    public event Action OnSavePlayerData;


    void IDataFromStorageToModelUpdater.UpdateModel()
    {
        readerWriter.ReadAllData(receivedData =>
        {
            IModelInteraction modelInteraction = dataHandlerInteraction.LastGamingSessions;
            modelInteraction.SetData(new PlayerGameData(CombineDataFromStorages(modelInteraction.GetData(), receivedData)));
        });
    }


    void IDataFromModelToStorageUpdater.UpdateStorage()
    {
        OnSavePlayerData?.Invoke();

        readerWriter.WriteAllData(new PlayerGameData(dataGetter));
    }


    void IDataReseter.Reset()
    {
        PlayerGameData newData = PlayerGameData.CreateDataWithDefaultValues();

        dataHandlerInteraction.CurrentGameSession.SetData(newData);
        readerWriter.WriteAllData(newData);
    }


    private IDataGetter CombineDataFromStorages(IDataGetter currentData, IDataGetter receivedData)
    {
        // currentData по определению не может быть null
        if (currentData == null) throw new ArgumentNullException(nameof(currentData));
        // receivedData может быть null
        if (receivedData == null) return currentData;

        if (currentData.Equals(receivedData)) return currentData;
        else
        {
            IDataGetter returnedDataGetter;

            if (currentData.Id != receivedData.Id)
            {
                // TODO: Вывод диалогового окна
                throw new NotImplementedException();
            }
            else
            {
                // TODO: скомбинировать значения полей по правилам.
                returnedDataGetter = new LastSessionsDataGetter(currentData, receivedData);
            }

            readerWriter.WriteAllData(new PlayerGameData(returnedDataGetter));
            return returnedDataGetter;
        }
    }
}
