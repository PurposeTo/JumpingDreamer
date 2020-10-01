using System;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine; // Не удалять, т.к. используется для платформозависимой компиляции


public class PlayerDataModelController : SingletonMonoBehaviour<PlayerDataModelController>
{
    private PlayerDataModel playerDataLocalModel;

    private PlayerDataLocalStorageSafe localStorageSafe = new PlayerDataLocalStorageSafe();
    private PlayerDataSynchronizer playerDataSynchronizer = new PlayerDataSynchronizer();

    private GPGSPlayerDataCloudStorage GPGSPlayerDataCloudStorage => GPGSServices.Instance.GPGSPlayerDataCloudStorage;
    public bool IsDataFileLoaded => localStorageSafe.IsDataFileLoaded;

    public static bool IsPlayerDataHaveAlreadyDeletedOrRestored { get; private set; } = false;

    public event Action OnSavePlayerStats;
    public event Action OnDeletePlayerData;
    public event Action OnRestoreDataFromCloud;


    public enum DataModelSelectionStatus
    {
        LocalModel,
        CloudModel
    }


    protected override void AwakeSingleton()
    {
        playerDataLocalModel = localStorageSafe.LoadPlayerData();
    }


    public PlayerDataModel GetPlayerDataModel() => playerDataLocalModel;


    public void UpdatePlayerModelAndSavePlayerData()
    {
        OnSavePlayerStats?.Invoke();
        SavePlayerData();
    }


    public void DeletePlayerData()
    {
        playerDataLocalModel = PlayerDataModel.CreateModelWithDefaultValues();

        localStorageSafe.DeletePlayerData();

        // Загрузка обновленных данных на облако
        GPGSPlayerDataCloudStorage.CreateSave(playerDataLocalModel);

        IsPlayerDataHaveAlreadyDeletedOrRestored = true;
        OnDeletePlayerData?.Invoke();
    }


    public void RestorePlayerDataFromCloud()
    {
        GPGSPlayerDataCloudStorage.ReadSavedGame((cloudModel, readingCloudDataStatus) =>
        {
            if (cloudModel != null)
            {
                playerDataLocalModel = cloudModel;

                localStorageSafe.IsDataFileLoaded = true;
                IsPlayerDataHaveAlreadyDeletedOrRestored = true;

                OnRestoreDataFromCloud?.Invoke();
            }
            else
            {
                if (readingCloudDataStatus == SavedGameRequestStatus.Success)
                {
                    Debug.Log("Полученные пустые данные с облака.");

                    playerDataLocalModel = PlayerDataModel.CreateModelWithDefaultValues();

                    localStorageSafe.IsDataFileLoaded = true;
                    IsPlayerDataHaveAlreadyDeletedOrRestored = true;

                    OnRestoreDataFromCloud?.Invoke();
                }
                else DialogWindowGenerator.Instance.CreateDialogWindow("Ошибка соединения!");
            }
        });
    }


    public void SynchronizePlayerDataStorages(PlayerDataModel cloudModel)
    {
        playerDataSynchronizer.SynchronizePlayerDataStorages(ref playerDataLocalModel, cloudModel);
    }


    public void OnDataModelSelected(PlayerDataModel selectedModel, DataModelSelectionStatus modelSelectionStatus)
    {
        playerDataSynchronizer.OnDataModelSelected(selectedModel, ref playerDataLocalModel, modelSelectionStatus);
    }


    private void SavePlayerData()
    {
        localStorageSafe.WritePlayerDataToFile(playerDataLocalModel);
        GPGSPlayerDataCloudStorage.CreateSave(playerDataLocalModel);
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
