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
            IModelInteraction modelInteraction = dataHandlerInteraction.GetInteractionWithDataOfLastGamingSessions();
            modelInteraction.SetData(CombineDataFromStorages(modelInteraction.GetData(), receivedData));
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

        dataHandlerInteraction.GetInteractionWithDataOfCurrentGameSession().SetData(newData);
        readerWriter.WriteAllData(newData);
    }


    private PlayerGameData CombineDataFromStorages(PlayerGameData data1, PlayerGameData data2)
    {
        if (data1 == null && data2 == null) throw new ArgumentNullException($"{nameof(data1)} and {nameof(data2)}");
        else if (data1 == null || data2 == null)
        {
            if (data1 == null) return data2;
            else return data1;
        }

        if (data1.Equals(data2)) return data1;

        if (data1.Id != data2.Id)
        {
            // TODO: Вывод диалогового окна
            throw new NotImplementedException();
        }
        else
        {
            // TODO: скомбинировать значения полей по правилам.
            throw new NotImplementedException();
        }

        // TODO: надо ли сохранить в хранилища данные после этого?
    }
}
