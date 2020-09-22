using System;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine; // Не удалять, т.к. используется для платформозависимой компиляции


public class PlayerDataModelController : SingletonMonoBehaviour<PlayerDataModelController>
{
    public PlayerDataModel PlayerDataLocalModel { get; private set; }

    private PlayerDataLocalStorageSafe localStorageSafe = new PlayerDataLocalStorageSafe();
    private PlayerDataSynchronizer playerDataSynchronizer = new PlayerDataSynchronizer();

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
        PlayerDataLocalModel = localStorageSafe.LoadPlayerData();
    }


    public void UpdatePlayerModelAndSavePlayerData()
    {
        OnSavePlayerStats?.Invoke();
        SavePlayerData();
    }


    public void DeletePlayerData()
    {
        PlayerDataLocalModel = PlayerDataModel.CreateModelWithDefaultValues();

        localStorageSafe.DeletePlayerData();

        // Загрузка обновленных данных на облако
        GPGSPlayerDataCloudStorage.Instance.CreateSave(PlayerDataLocalModel);

        IsPlayerDataHaveAlreadyDeletedOrRestored = true;
        OnDeletePlayerData?.Invoke();
    }


    public void RestorePlayerDataFromCloud()
    {
        GPGSPlayerDataCloudStorage.Instance.ReadSavedGame((cloudModel, readingCloudDataStatus) =>
        {
            if (cloudModel != null)
            {
                PlayerDataLocalModel = cloudModel;

                localStorageSafe.IsDataFileLoaded = true;
                IsPlayerDataHaveAlreadyDeletedOrRestored = true;

                OnRestoreDataFromCloud?.Invoke();
            }
            else
            {
                if (readingCloudDataStatus == SavedGameRequestStatus.Success)
                {
                    Debug.Log("Полученные пустые данные с облака.");

                    PlayerDataLocalModel = PlayerDataModel.CreateModelWithDefaultValues();

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
        playerDataSynchronizer.SynchronizePlayerDataStorages(PlayerDataLocalModel, cloudModel);
    }


    public void OnDataModelSelected(PlayerDataModel selectedModel, DataModelSelectionStatus modelSelectionStatus)
    {
        playerDataSynchronizer.OnDataModelSelected(selectedModel, PlayerDataLocalModel, modelSelectionStatus);
    }


    private void SavePlayerData()
    {
        localStorageSafe.WritePlayerDataToFile(PlayerDataLocalModel);
        GPGSPlayerDataCloudStorage.Instance.CreateSave(PlayerDataLocalModel);
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
