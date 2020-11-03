using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public delegate void LocalizationСhange();

public class LocalizationManager : SingletonMonoBehaviour<LocalizationManager>
{
    public event LocalizationСhange OnLocalizationChange;

    private int langIndex = 1;
    private string[] langArray = { "ru_Ru", "en_US" };

    private Dictionary<string, string> localizedText = new Dictionary<string, string>();
    private bool isReady = false;
    public const string missingTextString = "Localized Text not found";


    protected override void AwakeSingleton()
    {
        StartCoroutine(WaitLoadingPlayerSettingsEnumerator());
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
        else UnityEngine.Debug.LogError($"Localization text does not contains key {key}");

        return result;
    }

    public bool GetIsReady()
    {
        return isReady;
    }


    private IEnumerator LoadLocalizedTextEnumerator()
    {
        string filePath = Path.Combine(Application.streamingAssetsPath,
                                       "Languages/" + PlayerSettingsStorage.Instance.PlayerSettings.Language + ".json");

        var reader = new UnityWebRequest(filePath);
        reader.downloadHandler = new DownloadHandlerBuffer();

        yield return reader.SendWebRequest();

        if (reader.error != null)
        {
            Debug.LogError(reader.error);
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


    private IEnumerator WaitLoadingPlayerSettingsEnumerator()
    {
        yield return new WaitUntil(() => PlayerSettingsStorage.Instance != null);
        yield return new WaitUntil(() => PlayerSettingsStorage.Instance.PlayerSettings != null);
        SetLanguageSettings();
        LoadLocalizedText();
    }
}
