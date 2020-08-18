using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;


public delegate void LocalizationСhange();

public class LocalizationManager : SingletonMonoBehaviour<LocalizationManager>
{

    private int langIndex = 1;
    private string[] langArray = { "ru_Ru", "en_US" };

    private Dictionary<string, string> localizedText;
    private bool isReady = false;
    private string missingTextString = "Localized Text not found";

    public event LocalizationСhange OnLocalizationChange;

    protected override void AwakeSingleton()
    {
        SetLanguageSettings();
        LocalizationPlatform();
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

    public void LoadLocalizedText()
    {
        localizedText = new Dictionary<string, string>();


        string filePath = Path.Combine(Application.streamingAssetsPath, "Languages/" + PlayerSettings.Language + ".json");

        Debug.Log($"Language data filePath: {filePath}");
        if (File.Exists(filePath))
        {
            string dataAsJson = File.ReadAllText(filePath);
            LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
            Debug.Log("Translate is done on Editor");


            for (int i = 0; i < loadedData.items.Length; i++)
            {
                localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            }

            Debug.Log("Data loaded, dictionary contains: " + localizedText.Count + " entries");
        }
        else
        {
            Debug.LogError("Can not find file!");
        }


        Debug.Log("Set language: " + Instance.GetLocalizedValue("Language"));
        isReady = true;
        if (OnLocalizationChange != null) OnLocalizationChange();
        Debug.Log("Change Text event");
    }


    IEnumerator LoadLocalizedTextOnAndroid()
    {
        localizedText = new Dictionary<string, string>();
        string filePath = Path.Combine(Application.streamingAssetsPath, "Languages/" + PlayerSettings.Language + ".json");
        WWW reader = new WWW(filePath);
        yield return reader;
        Debug.LogWarning(filePath);
        if (reader.error != null)
        {
            Debug.LogWarning(reader.error);
            yield break;
        } 

        string dataAsJson = reader.text;
        
        LocalizationData loadedData = JsonUtility.FromJson<LocalizationData>(dataAsJson);
        Debug.Log("Translate is done on Android");

        for (int i = 0; i < loadedData.items.Length; i++)
        {
            localizedText.Add(loadedData.items[i].key, loadedData.items[i].value);
            //Debug.Log("KEYS:" + loadedData.items[i].key);
        }


        Debug.Log("Set language: " + Instance.GetLocalizedValue("language"));

        isReady = true;
        if (OnLocalizationChange != null) OnLocalizationChange();
        Debug.Log("Change Text event");
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

        LocalizationPlatform();
    }


    public void LocalizationPlatform()
    {
        if (Application.platform == RuntimePlatform.WindowsEditor)
            LoadLocalizedText();
        else if (Application.platform == RuntimePlatform.Android)
            StartCoroutine(LoadLocalizedTextOnAndroid());
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
}
