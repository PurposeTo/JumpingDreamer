using System;
using System.Collections;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine; // Не удалять, т.к. используется для платформозависимой компиляции


public class PlayerDataModelController : SingletonSuperMonoBehaviour<PlayerDataModelController>
{
    public static bool IsPlayerDataHaveAlreadyDeletedOrRestored { get; private set; } = false;

    public event Action OnSavePlayerStats;
    public event Action OnDeletePlayerData;
    public event Action OnRestoreDataFromCloud;

    private PlayerDataModel playerDataModel;
    private PlayerDataSynchronizer playerDataSynchronizer;

    private ICoroutineInfo synchronizePlayerDataStoragesInfo;


    public enum DataModelSelectionStatus
    {
        LocalModel,
        CloudModel
    }


    protected override void AwakeSingleton()
    {
        playerDataSynchronizer = new PlayerDataSynchronizer(this);
        synchronizePlayerDataStoragesInfo = CreateCoroutineInfo(SynchronizePlayerDataStorages());
        ContiniousCoroutineExecution(ref synchronizePlayerDataStoragesInfo);
    }


    public PlayerDataModel GetPlayerDataModel() => playerDataModel;


    public void UpdatePlayerModelAndSavePlayerData()
    {
        OnSavePlayerStats?.Invoke();
        SavePlayerData();
    }


    public void DeletePlayerData()
    {
        playerDataModel = PlayerDataModel.CreateModelWithDefaultValues();

        localStorageSafe.DeletePlayerData();

        // Загрузка обновленных данных на облако
        GPGSPlayerDataCloudStorage.CreateSave(playerDataModel);

        IsPlayerDataHaveAlreadyDeletedOrRestored = true;
        OnDeletePlayerData?.Invoke();
    }


    public void RestorePlayerDataFromCloud()
    {
        GPGSPlayerDataCloudStorage.ReadSavedGame((cloudModel, readingCloudDataStatus) =>
        {
            if (cloudModel != null)
            {
                playerDataModel = cloudModel;

                localStorageSafe.IsDataFileLoaded = true;
                IsPlayerDataHaveAlreadyDeletedOrRestored = true;

                OnRestoreDataFromCloud?.Invoke();
            }
            else
            {
                if (readingCloudDataStatus == SavedGameRequestStatus.Success)
                {
                    Debug.Log("Полученные пустые данные с облака.");

                    playerDataModel = PlayerDataModel.CreateModelWithDefaultValues();

                    localStorageSafe.IsDataFileLoaded = true;
                    IsPlayerDataHaveAlreadyDeletedOrRestored = true;

                    OnRestoreDataFromCloud?.Invoke();
                }
                else PopUpWindowGenerator.Instance.CreateDialogWindow("Ошибка соединения!");
            }
        });
    }


    public void OnDataModelSelected(PlayerDataModel selectedModel, DataModelSelectionStatus modelSelectionStatus)
    {
        playerDataSynchronizer.OnDataModelSelected(selectedModel, ref playerDataModel, modelSelectionStatus);
    }


    private IEnumerator SynchronizePlayerDataStorages()
    {
        playerDataSynchronizer.StartGetSynchronizedPlayerDataModelCoroutine(synchronizedPlayerDataModel =>
        {
            this.playerDataModel = synchronizedPlayerDataModel;
        });
        yield return new WaitWhile(() => playerDataSynchronizer.IsGetSynchronizedPlayerDataModelCoroutineExecuting());
    }


    private void SavePlayerData()
    {
        localStorageSafe.WritePlayerDataToFile(playerDataModel);
        GPGSPlayerDataCloudStorage.CreateSave(playerDataModel);
    }


#if UNITY_EDITOR

    private void OnApplicationQuit()
    {
        SavePlayerData();
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
