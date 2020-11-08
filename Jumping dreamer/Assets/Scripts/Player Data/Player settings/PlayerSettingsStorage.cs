using System.IO;
using UnityEngine;

public class PlayerSettingsStorage : SingletonSuperMonoBehaviour<PlayerSettingsStorage>
{
    public PlayerSettingsModel PlayerSettings { get; private set; }

    private string filePath;


    protected override void AwakeSingleton()
    {
        filePath = DataLoaderHelper.GetFilePath(PlayerSettingsModel.FileName);
        PlayerSettings = LoadPlayerSettings();
    }


    private PlayerSettingsModel LoadPlayerSettings()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            Debug.Log($"PlayerSettingsModel json = {json}");
            return JsonUtility.FromJson<PlayerSettingsModel>(json);
        }
        else return new PlayerSettingsModel();
    }


    private void WriteDataToFile()
    {
        File.WriteAllText(filePath, JsonUtility.ToJson(PlayerSettings));
    }


#if UNITY_EDITOR

    private void OnApplicationQuit()
    {
        WriteDataToFile();
    }

#elif UNITY_ANDROID

        private void OnApplicationPause(bool pause)
        {
            Debug.Log($"OnApplicationPause code: {pause}");
            if (pause)
            {
                WriteDataToFile();
            }
        }

#endif
}
