using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;


public class LocalizationManager : SingletonMonoBehaviour<LocalizationManager>
{
    public event Action OnLocalizationChange;

    private int langIndex = 1;
    private string[] langArray = { "ru_Ru", "en_US" };

    private Dictionary<string, string> localizedText = new Dictionary<string, string>();
    private bool isReady = false;
    public const string missingTextString = "Localized Text not found";


    protected override void AwakeSingleton()
    {
        PlayerSettingsStorage.SetCommandToQueue(SetLanguageSettings, LoadLocalizedText);
    }


    private void SetLanguageSettings()
    {
        if (!Array.Exists(langArray, item => item == PlayerSettingsStorage.Instance.PlayerSettings.Language))
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Russian:
                    PlayerSettingsStorage.Instance.PlayerSettings.Language = "ru_Ru";
                    break;
                case SystemLanguage.English:
                    PlayerSettingsStorage.Instance.PlayerSettings.Language = "en_Us";
                    break;
                default:
                    PlayerSettingsStorage.Instance.PlayerSettings.Language = "en_Us";
                    break;
            }
        }

        Debug.Log($"LanguageSettings is now {PlayerSettingsStorage.Instance.PlayerSettings.Language}!");
        SetRightLangIndex();
    }


    private void SetRightLangIndex()
    {
        // Todo - зачем это?
        for (int i = 0; i < langArray.Length; i++)
        {
            if (PlayerSettingsStorage.Instance.PlayerSettings.Language == langArray[i])
            {
                langIndex = i + 1;
            }
        }
    }


    public void SetLanguage()
    {
        isReady = false;

        if (langIndex != langArray.Length)
        {
            langIndex++;
        }
        else langIndex = 1;
        PlayerSettingsStorage.Instance.PlayerSettings.Language = langArray[langIndex - 1];

        LoadLocalizedText();
    }


    public void LoadLocalizedText()
    {
        StartCoroutine(LoadLocalizedTextEnumerator());
    }


    public string GetLocalizedValue(string key)
    {
        string result = missingTextString;
        if (localizedText.ContainsKey(key))
        {
            result = localizedText[key];
        }
        else
        {
            if (localizedText.Count != 0) UnityEngine.Debug.LogError($"Localization text does not contains key {key}");
            else UnityEngine.Debug.LogWarning($"Localization text does not contains key because localizedText dictionary does not initialize!");
        }

        return result;
    }

    public bool GetIsReady()
    {
        return isReady;
    }


    private IEnumerator LoadLocalizedTextEnumerator()
    {
        Debug.Log("Loading localized text!");

        string filePath = Path.Combine(Application.streamingAssetsPath,
                                       "Languages/" + PlayerSettingsStorage.Instance.PlayerSettings.Language + ".json");

        if (!File.Exists(filePath))
        {
            Debug.LogError($"LocalizationManager can't find file by filepath: {filePath}");
            // Не делать return! reader может каким то чудным образом найти файл!
        }

        var reader = new UnityWebRequest(filePath);
        reader.downloadHandler = new DownloadHandlerBuffer();

        yield return reader.SendWebRequest();

        if (reader.error != null)
        {
            Debug.LogError($"Localization reader exit with error {reader.error}");
            yield break;
        }

        string dataAsJson = reader.downloadHandler.text;

        reader.Dispose();

        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        Debug.Log("Translate is done!");

        Array.ForEach(loadedData.items, localizedItem =>
        {
            localizedText[localizedItem.key] = localizedItem.value;
            Debug.Log("KEYS:" + localizedItem.key);
        });

        Debug.Log("Set language: " + Instance.GetLocalizedValue("Language"));

        isReady = true;
        OnLocalizationChange?.Invoke();
        Debug.Log("Change Text event");
    }

}
