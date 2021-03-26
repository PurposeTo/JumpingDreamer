using System;
using System.Collections;
using Desdiene.Singleton;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine; // Не удалять, т.к. используется для платформозависимой компиляции

public class PlayerDataModelController : SingletonSuperMonoBehaviour<PlayerDataModelController>
{
    public IDataInteraction DataInteraction { get; private set; }
    public IDataFromStorageToModelUpdater DataFromStorageToModelUpdater { get; private set; }
    public IDataFromModelToStorageUpdater DataFromModelToStorageUpdater { get; private set; }
    public IDataReseter DataReseter { get; private set; }
    public ISynchronizerNotifier SynchronizerNotifier { get; private set; }


    protected override void AwakeSingleton()
    {
        DataHandler dataHandler = new DataHandler(new DataModel(), new DataModel());
        DataInteraction = dataHandler;
        StorageDataReaderWriter storageDataReaderWriter = new DataLoader
            (this, new LocalDataStorage(this), new CloudDataStorage(this));

        DataSynchronizer dataSynchronizer = new DataSynchronizer(dataHandler, DataInteraction.Getter, storageDataReaderWriter);
        DataFromStorageToModelUpdater = dataSynchronizer;
        DataFromModelToStorageUpdater = dataSynchronizer;
        DataReseter = dataSynchronizer;
        SynchronizerNotifier = dataSynchronizer;
    }

    // Не забывать вносить изменения в случае их возникновения
    #region Платформозависимое сохранение
#if UNITY_EDITOR

    private void OnApplicationQuit()
    {
        DataFromModelToStorageUpdater.UpdateStorage();
    }

#elif UNITY_ANDROID

    private void OnApplicationPause(bool pause)
    {
        Debug.Log($"OnApplicationPause code: {pause}");
        if (pause)
        {
            dataFromModelToStorageUpdater.UpdateStorage();
        }
    }

#endif
    #endregion
}
