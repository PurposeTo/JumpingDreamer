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


    protected override void AwakeSingleton()
    {
        GPGSPlayerDataCloudStorage = new GPGSPlayerDataCloudStorage(this);
        playerDataSynchronizer = new PlayerDataSynchronizer(this, localStorageSafe, GPGSPlayerDataCloudStorage);
        synchronizePlayerDataStoragesInfo = CreateCoroutineContainer();
        ExecuteCoroutineContinuously(ref synchronizePlayerDataStoragesInfo, SynchronizePlayerDataModel());
    }


    #region Платформозависимое сохранение
#if UNITY_EDITOR

    private void OnApplicationQuit()
    {
        SavePlayerDataToAllStorages();
    }

    // Не забывать вносить изменения в случае их возникновения
#elif UNITY_ANDROID

        private void OnApplicationPause(bool pause)
        {
            Debug.Log($"OnApplicationPause code: {pause}");
            if (pause)
            {
                SavePlayerDataToAllStorages();
            }
        }

#endif
    #endregion


    public IGetDataModel GetGettableDataModel()
    {
        TryToUsePlayerDataModel(out PlayerDataModel playerDataModel);
        return playerDataModel;
    }


    public ISetDataModel GetSettableDataModel()
    {
        TryToUsePlayerDataModel(out PlayerDataModel playerDataModel);
        return playerDataModel;
    }


    public void ResetPlayerData()
    {
        if (TryToUsePlayerDataModel(out PlayerDataModel playerDataModel))
        {
            this.playerDataModel = PlayerDataModel.CreateModelWithDefaultValues();

            SavePlayerDataToAllStorages();
            IsPlayerDataHaveAlreadyDeletedOrRestored = true;
            OnResetPlayerData?.Invoke();
        }
    }


    public void RestorePlayerDataFromCloud()
    {
        if (TryToUsePlayerDataModel(out PlayerDataModel playerDataModel))
        {
            GPGSPlayerDataCloudStorage.ReadData((cloudModel, readingCloudDataStatus) =>
            {
                if (cloudModel != null)
                {
                    this.playerDataModel = cloudModel;

                    SavePlayerDataToLocalFile();
                    IsPlayerDataHaveAlreadyDeletedOrRestored = true;
                    OnRestoreDataFromCloud?.Invoke();
                }
                else
                {
                    if (readingCloudDataStatus == SavedGameRequestStatus.Success)
                    {
                        Debug.Log("Из облака полученные пустые данные.");

                        this.playerDataModel = PlayerDataModel.CreateModelWithDefaultValues();

                        SavePlayerDataToAllStorages();
                        IsPlayerDataHaveAlreadyDeletedOrRestored = true;
                        OnRestoreDataFromCloud?.Invoke();
                    }
                    else PopUpWindowGenerator.Instance.CreateDialogWindow("Ошибка соединения!");
                }
            });
        }
    }


    public void UpdatePlayerModelAndSavePlayerData()
    {
        OnSavePlayerStats?.Invoke();
        SavePlayerDataToAllStorages();
    }


    private IEnumerator SynchronizePlayerDataModel()
    {
        bool isPlayerDataModelSynchronizing = true;

        playerDataSynchronizer.StartSynchronizingPlayerDataModel((synchronizedPlayerDataModel) =>
        {
            if (synchronizedPlayerDataModel is null) throw new ArgumentNullException(nameof(synchronizedPlayerDataModel));

            playerDataModel = synchronizedPlayerDataModel;
            isPlayerDataModelSynchronizing = false;
        });

        yield return new WaitWhile(() => isPlayerDataModelSynchronizing);
    }


    private void SavePlayerDataToLocalFile()
    {
        if (TryToUsePlayerDataModel(out PlayerDataModel playerDataModel))
        {
            localStorageSafe.SaveDataToFileAndEncrypt(playerDataModel);
        }
    }


    private void SavePlayerDataToCloud()
    {
        if (TryToUsePlayerDataModel(out PlayerDataModel playerDataModel))
        {
            GPGSPlayerDataCloudStorage.SaveData(playerDataModel);
        }
    }


    private void SavePlayerDataToAllStorages()
    {
        SavePlayerDataToLocalFile();
        SavePlayerDataToCloud();
    }


    private bool TryToUsePlayerDataModel(out PlayerDataModel playerDataModel)
    {
        playerDataModel = this.playerDataModel;
        bool isModelNull = playerDataModel is null;

        if (isModelNull)
        {
            if (synchronizePlayerDataStoragesInfo.IsExecuting) Debug.LogError("Синхронизация не была завершена. PlayerDataModel is null");
            else throw new NullReferenceException($"{nameof(playerDataModel)} is null");
        }

        return !isModelNull;
    }
}
