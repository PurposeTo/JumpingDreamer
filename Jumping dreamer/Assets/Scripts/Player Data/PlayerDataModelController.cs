using System;
using System.Collections;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine; // Не удалять, т.к. используется для платформозависимой компиляции


public class PlayerDataModelController : SingletonSuperMonoBehaviour<PlayerDataModelController>
{
    public static bool IsPlayerDataHaveAlreadyDeletedOrRestored { get; private set; } = false;

    public event Action OnSavePlayerStats;
    public event Action OnResetPlayerData;
    public event Action OnRestoreDataFromCloud;

    private PlayerDataModel playerDataModel;
    private GPGSPlayerDataCloudStorage GPGSPlayerDataCloudStorage;
    private readonly PlayerDataLocalStorageSafe localStorageSafe = new PlayerDataLocalStorageSafe();
    private PlayerDataSynchronizer playerDataSynchronizer;


    private ICoroutineContainer synchronizePlayerDataStoragesInfo;


    //public enum DataModelSelectionStatus
    //{
    //    LocalModel,
    //    CloudModel
    //}


    protected override void AwakeSingleton()
    {
        playerDataSynchronizer = new PlayerDataSynchronizer(this);
        GPGSPlayerDataCloudStorage = new GPGSPlayerDataCloudStorage(this);
        synchronizePlayerDataStoragesInfo = CreateCoroutineContainer();
        ExecuteCoroutineContinuously(ref synchronizePlayerDataStoragesInfo, GetSynchronizedPlayerDataModel());
    }


    public PlayerDataModel GetPlayerDataModel() => playerDataModel;


    public void ResetPlayerData()
    {
        playerDataModel = PlayerDataModel.CreateModelWithDefaultValues();

        SavePlayerDataToAllStorages();
        IsPlayerDataHaveAlreadyDeletedOrRestored = true;
        OnResetPlayerData?.Invoke();
    }


    public void RestorePlayerDataFromCloud()
    {
        GPGSPlayerDataCloudStorage.ReadDataFromCloud((cloudModel, readingCloudDataStatus) =>
        {
            if (cloudModel != null)
            {
                playerDataModel = cloudModel;

                SavePlayerDataToLocalFile();
                IsPlayerDataHaveAlreadyDeletedOrRestored = true;
                OnRestoreDataFromCloud?.Invoke();
            }
            else
            {
                if (readingCloudDataStatus == SavedGameRequestStatus.Success)
                {
                    Debug.Log("Из облака полученные пустые данные."); // !!!!!!!!!!!! А ПОЧЕМУ? (типа если данных нет на облаке?) !!!!!!!!!!!

                    playerDataModel = PlayerDataModel.CreateModelWithDefaultValues();

                    SavePlayerDataToLocalFile();
                    IsPlayerDataHaveAlreadyDeletedOrRestored = true;
                    OnRestoreDataFromCloud?.Invoke();
                }
                else PopUpWindowGenerator.Instance.CreateDialogWindow("Ошибка соединения!");
            }
        });
    }


    public void UpdatePlayerModelAndSavePlayerData()
    {
        OnSavePlayerStats?.Invoke();
        SavePlayerDataToAllStorages();
    }


    //public void OnDataModelSelected(PlayerDataModel selectedModel, DataModelSelectionStatus modelSelectionStatus)
    //{
    //    playerDataSynchronizer.OnDataModelSelected(selectedModel, ref playerDataModel, modelSelectionStatus);
    //}


    // Coroutine не гарантирует, что модель будет получена до окончания её выполнения
    private IEnumerator GetSynchronizedPlayerDataModel()
    {
        playerDataSynchronizer.StartGetSynchronizedPlayerDataModelCoroutine(localStorageSafe, GPGSPlayerDataCloudStorage, (synchronizedPlayerDataModel, loadedPlayerDataModel) =>
        {
            SynchronizePlayerDataStorages(synchronizedPlayerDataModel, loadedPlayerDataModel);
        });

        yield return new WaitWhile(() => playerDataSynchronizer.IsGetSynchronizedPlayerDataModelCoroutineExecuting());
    }


    private void SynchronizePlayerDataStorages(PlayerDataModel synchronizedPlayerDataModel, LoadedPlayerDataModel loadedPlayerDataModel)
    {
        playerDataModel = synchronizedPlayerDataModel;

        switch (loadedPlayerDataModel)
        {
            case LoadedPlayerDataModel.LocalModel:
                SavePlayerDataToCloud();
                break;
            case LoadedPlayerDataModel.CloudModel:
                SavePlayerDataToLocalFile();
                break;
            case LoadedPlayerDataModel.CombinedModel:
                SavePlayerDataToAllStorages();
                break;
            case LoadedPlayerDataModel.Null:
                playerDataModel = PlayerDataModel.CreateModelWithDefaultValues();
                SavePlayerDataToAllStorages();
                break;
            default:
                Debug.LogError("Ошибка синхронизации данных.");
                break;
        }
    }


    private void SavePlayerDataToLocalFile() => localStorageSafe.SaveDataToFileAndEncrypt(playerDataModel);


    private void SavePlayerDataToCloud() => GPGSPlayerDataCloudStorage.SaveDataToCloud(playerDataModel);


    private void SavePlayerDataToAllStorages()
    {
        SavePlayerDataToLocalFile();
        SavePlayerDataToCloud();
    }


#if UNITY_EDITOR

    private void OnApplicationQuit()
    {
        SavePlayerDataToAllStorages();
    }

#elif UNITY_ANDROID

        private void OnApplicationPause(bool pause)
        {
            Debug.Log($"OnApplicationPause code: {pause}");
            if (pause)
            {
                SavePlayerData();
            }
        }

#endif
}
