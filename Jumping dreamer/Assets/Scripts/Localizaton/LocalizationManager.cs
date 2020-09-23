using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public delegate void LocalizationСhange();

public class LocalizationManager : SingletonMonoBehaviour<LocalizationManager>
{

    private int langIndex = 1;
    private string[] langArray = { "ru_Ru", "en_US" };

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private const string missingTextString = "Localized Text not found";

    public event LocalizationСhange OnLocalizationChange;

    protected override void AwakeSingleton()
    {
        SetLanguageSettings();
        LoadLocalizedText();
    }


    private void SetLanguageSettings()
    {
        if (string.IsNullOrEmpty(PlayerSettings.Language))
        {
            if (Application.systemLanguage == SystemLanguage.Russian)
            {
                PlayerSettings.Language = "ru_Ru";
            }
            else PlayerSettings.Language = "en_Us";
        }

        SetRightLangIndex();
    }


    private void SetRightLangIndex()
    {
        // Todo - зачем это?
        for (int i = 0; i < langArray.Length; i++)
        {
            if (PlayerSettings.Language == langArray[i])
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
        PlayerSettings.Language = langArray[langIndex - 1];

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

        return result;
    }

    public bool GetIsReady()
    {
        return isReady;
    }


    private IEnumerator LoadLocalizedTextEnumerator()
    {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, "Languages/" + PlayerSettings.Language + ".json");

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

        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            //Debug.Log("KEYS:" + loadedData.items[i].key);
        }


        Debug.Log("Set language: " + Instance.GetLocalizedValue("language"));

        isReady = true;
        OnLocalizationChange?.Invoke();
        Debug.Log("Change Text event");
    }
}
