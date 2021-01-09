﻿using System;
using System.Collections;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine; // Не удалять, т.к. используется для платформозависимой компиляции

public class PlayerDataModelController : SingletonSuperMonoBehaviour<PlayerDataModelController>
{
    public static bool IsPlayerDataHaveAlreadyDeletedOrRestored { get; private set; } = false;

    public event Action OnSavePlayerStats;
    public event Action OnResetPlayerData;
    public event Action OnRestoreDataFromCloud;

    private PlayerModel playerModel = new PlayerModel();
    private GPGSPlayerDataCloudStorage GPGSPlayerDataCloudStorage;
    private readonly PlayerDataLocalStorageSafe localStorageSafe = new PlayerDataLocalStorageSafe();
    private PlayerDataSynchronizer playerDataSynchronizer;

    private ICoroutineContainer synchronizePlayerDataStoragesInfo;


    protected override void AwakeSingleton()
    {
        GPGSPlayerDataCloudStorage = new GPGSPlayerDataCloudStorage(this);
        playerDataSynchronizer = new PlayerDataSynchronizer(this, localStorageSafe, GPGSPlayerDataCloudStorage);
        synchronizePlayerDataStoragesInfo = CreateCoroutineContainer();

        DisplayerOfLoading.InitializedInstance += (instance) =>
        {
            instance.StartWaiting(this);

            // Для editor'a
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                playerModel.SetDataWithDefaultValues();
                instance.EndWaiting(this);
            }

            ExecuteCoroutineContinuously(ref synchronizePlayerDataStoragesInfo, SynchronizePlayerDataModel());
        };
    }

    // Не забывать вносить изменения в случае их возникновения
    #region Платформозависимое сохранение
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
                SavePlayerDataToAllStorages();
            }
        }

#endif
    #endregion


    public IGetAction GetAction() => playerModel.GetPlayerStats();


    public IGetModelData GetGettableDataModel()
    {
        TryToUsePlayerDataModel(out PlayerModelData modelData);
        return modelData;
    }


    public ISetDataModel GetSettableDataModel()
    {
        TryToUsePlayerDataModel(out PlayerModelData modelData);
        return playerModel;
    }


    public void ResetPlayerData()
    {
        playerModel.SetDataWithDefaultValues();

        SavePlayerDataToAllStorages();
        IsPlayerDataHaveAlreadyDeletedOrRestored = true;
        OnResetPlayerData?.Invoke();
    }


    public void RestorePlayerDataFromCloud()
    {

        GPGSPlayerDataCloudStorage.ReadData((cloudData, readingCloudDataStatus) =>
        {
            if (cloudData != null)
            {
                playerModel.SetData(cloudData);

                SavePlayerDataToLocalFile();
                IsPlayerDataHaveAlreadyDeletedOrRestored = true;
                OnRestoreDataFromCloud?.Invoke();
            }
            else
            {
                if (readingCloudDataStatus == SavedGameRequestStatus.Success)
                {
                    Debug.Log("Из облака полученные пустые данные.");

                    playerModel.SetDataWithDefaultValues();

                    SavePlayerDataToAllStorages();
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


    private IEnumerator SynchronizePlayerDataModel()
    {
        bool isPlayerDataModelSynchronizing = true;

        playerDataSynchronizer.StartSynchronizingPlayerDataModel((synchronizedPlayerDataModel) =>
        {
            if (synchronizedPlayerDataModel == null)
            {
                playerModel.SetDataWithDefaultValues();
                SavePlayerDataToAllStorages();
            }
            else playerModel.SetData(synchronizedPlayerDataModel);

            isPlayerDataModelSynchronizing = false;
        });

        yield return new WaitWhile(() => isPlayerDataModelSynchronizing);
        DisplayerOfLoading.Instance.EndWaiting(this);
    }


    private void SavePlayerDataToLocalFile()
    {
        if (TryToUsePlayerDataModel(out PlayerModelData modelData))
        {
            localStorageSafe.SaveDataToFileAndEncrypt(modelData);
        }
    }


    private void SavePlayerDataToCloud()
    {
        if (TryToUsePlayerDataModel(out PlayerModelData modelData))
        {
            GPGSPlayerDataCloudStorage.SaveData(modelData);
        }
    }


    private void SavePlayerDataToAllStorages()
    {
        SavePlayerDataToLocalFile();
        SavePlayerDataToCloud();
    }


    private bool TryToUsePlayerDataModel(out PlayerModelData modelData)
    {
        modelData = playerModel.GetData();
        bool isDataNull = modelData == null;

        if (isDataNull)
        {
            if (synchronizePlayerDataStoragesInfo.IsExecuting) Debug.LogError("Синхронизация не была завершена. DataModel == null.");
            else throw new NullReferenceException($"{nameof(modelData)} == null");
        }

        return !isDataNull;
    }
}
