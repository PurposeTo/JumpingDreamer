using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;
using Debug = UnityEngine.Debug;


public class LocalizationManager : SingletonSuperMonoBehaviour<LocalizationManager>
{
    public event Action OnLocalizationChange;

    private int langIndex = 1;
    private string[] langArray = { "ru_Ru", "en_US" };

    private readonly Dictionary<string, string> localizedText = new Dictionary<string, string>();
    private bool isReady = false;
    public const string missingTextString = "Localized Text not found";

    private ICoroutineInfo loadLocalizedTextInfo;


    protected override void AwakeSingleton()
    {
        PlayerSettingsStorage.InitializedInstance += (Instance) =>
        {
            SetLanguageSettings(Instance.PlayerSettings);
            loadLocalizedTextInfo = CreateCoroutineInfo();
            LoadLocalizedText();
        };
    }


    private void SetLanguageSettings(PlayerSettingsModel playerSettings)
    {
        if (!Array.Exists(langArray, item => item == playerSettings.Language))
        {
            switch (Application.systemLanguage)
            {
                case SystemLanguage.Russian:
                    playerSettings.Language = "ru_Ru";
                    break;
                case SystemLanguage.English:
                    playerSettings.Language = "en_Us";
                    break;
                default:
                    playerSettings.Language = "en_Us";
                    break;
            }
        }

        Debug.Log($"LanguageSettings is now {playerSettings.Language}!");
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
        ContiniousCoroutineExecution(ref loadLocalizedTextInfo, LoadLocalizedTextEnumerator());
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
            if (localizedText.Count != 0) Debug.LogError($"Localization text does not contains key {key}");
            else Debug.LogWarning($"Localization text does not contains key because localizedText dictionary does not initialize!");
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

        UnityWebRequest reader = new UnityWebRequest(filePath) { downloadHandler = new DownloadHandlerBuffer() };

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
            //Debug.Log("KEYS:" + localizedItem.key);
        });

        Debug.Log("Set language: " + Instance.GetLocalizedValue("Language"));

        isReady = true;
        OnLocalizationChange?.Invoke();
        Debug.Log("Change Text event");
    }

}
