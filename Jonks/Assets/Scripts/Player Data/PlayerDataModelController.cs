using System;
using System.Runtime.InteropServices;
using UnityEngine; // Не удалять, т.к. используется для платформозависимой компиляции


public class PlayerDataModelController : SingletonMonoBehaviour<PlayerDataModelController>
{
    public PlayerDataModel PlayerDataLocalModel { get; private set; }

    private PlayerDataLocalStorageSafe localStorageSafe = new PlayerDataLocalStorageSafe();
    private PlayerDataSynchronizer playerDataSynchronizer = new PlayerDataSynchronizer();

    public bool IsDataFileLoaded => localStorageSafe.IsDataFileLoaded;

    // TODO: Включать bool когда данные с облака были успешно (Интернет соединение было и т.п.) удалены
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


    public void SynchronizePlayerDataStorages(PlayerDataModel cloudModel)
    {
        playerDataSynchronizer.SynchronizePlayerDataStorages(PlayerDataLocalModel, cloudModel);
    }


    public void RestorePlayerDataFromCloud()
    {
        PlayerDataModel cloudModel = GPGSPlayerDataCloudStorage.Instance.ReadSavedGame(PlayerDataModel.FileName);

        if (cloudModel != null)
        {
            PlayerDataLocalModel = cloudModel;
        }
        else
        {
            // TODO: Сделать проверку. Если данных на облаке не было, то создать пустую модель.
            // Если не было соединения с интернетом, то вывести окно "Ошибка соединения!" и НИЧЕГО НЕ ДЕЛАТЬ
            PlayerDataLocalModel = PlayerDataModel.CreateModelWithDefaultValues();
        }
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
