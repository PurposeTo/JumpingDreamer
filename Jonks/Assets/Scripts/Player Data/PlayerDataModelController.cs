using System;
using UnityEngine; // Не удалять, т.к. используется для платформозависимой компиляции


public class PlayerDataModelController : SingletonMonoBehaviour<PlayerDataModelController>
{
    public PlayerDataModel PlayerDataLocalModel { get; private set; }

    private PlayerDataLocalStorageSafe localStorageSafe = new PlayerDataLocalStorageSafe();
    private PlayerDataSynchronizer playerDataSynchronizer = new PlayerDataSynchronizer();

    public bool IsDataFileLoaded => localStorageSafe.IsDataFileLoaded;

    public static bool IsPlayerDataAlreadyReset { get; private set; } = false;

    public event Action OnSavePlayerStats;
    public event Action OnDeletePlayerData;


    protected override void AwakeSingleton()
    {
        PlayerDataLocalModel = localStorageSafe.LoadPlayerData();
    }


    public void UpdatePlayerModelAndSavePlayerData()
    {
        OnSavePlayerStats?.Invoke();
        SavePlayerData();
    }


    public void SavePlayerData()
    {
        localStorageSafe.WritePlayerDataToFile(PlayerDataLocalModel);
        GPGSPlayerDataCloudStorage.Instance.CreateSave(PlayerDataLocalModel);
    }


    public void DeletePlayerData()
    {
        PlayerDataLocalModel = PlayerDataModel.CreateModelWithDefaultValues();
        localStorageSafe.DeletePlayerData();
        GPGSPlayerDataCloudStorage.Instance.CreateSave(PlayerDataLocalModel);

        IsPlayerDataAlreadyReset = true;
        OnDeletePlayerData?.Invoke();
    }


    public void RestorePlayerDataFromCloud()
    {
        PlayerDataModel cloudModel = GPGSPlayerDataCloudStorage.Instance.ReadSavedGame(PlayerDataModel.FileName, out GPGSPlayerDataCloudStorage.ReadingCloudDataStatus readingCloudDataStatus);

        if (cloudModel != null)
        {
            PlayerDataLocalModel = cloudModel;
            IsPlayerDataAlreadyReset = true;
        }
        else
        {
            if (readingCloudDataStatus == GPGSPlayerDataCloudStorage.ReadingCloudDataStatus.NoDataOnCloud)
            {
                PlayerDataLocalModel = PlayerDataModel.CreateModelWithDefaultValues();
                IsPlayerDataAlreadyReset = true;
                return;
            }
            else if (readingCloudDataStatus == GPGSPlayerDataCloudStorage.ReadingCloudDataStatus.InternetNotAvaliable)
            {
                DialogWindowGenerator.Instance.CreateDialogWindow("Ошибка соединения!");
                return;
            }
        }
    }


    public void SynchronizePlayerDataStorages(PlayerDataModel cloudModel)
    {
        playerDataSynchronizer.SynchronizePlayerDataStorages(
            PlayerDataLocalModel,
            cloudModel,
            () => StartCoroutine(playerDataSynchronizer.WaitForPlayerChooseEnumerator(PlayerDataLocalModel, cloudModel)));
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
