using System.Text;
using UnityEngine; // Не удалять, т.к. используется для платформозависимой компиляции
using System;

public class PlayerDataSaver : SingletonMonoBehaviour<PlayerDataSaver>
{
    public event Action OnSavePlayerStats;


    public void SavePlayerData()
    {
        PlayerDataLocalStorageSafe.Instance.WritePlayerDataToFile();
        GPGSPlayerDataCloudStorage.Instance.CreateSave(
            Encoding.UTF8.GetBytes(JsonConverterWrapper.SerializeObject(PlayerDataLocalStorageSafe.Instance.PlayerDataModel, null)));
    }


    public void UpdatePlayerModelAndSavePlayerData()
    {
        OnSavePlayerStats?.Invoke();
        SavePlayerData();
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
