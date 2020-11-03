using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


[RequireComponent(typeof(TMP_Text))]
public class LocalizedText : MonoBehaviour
{

    public string key;

    private TMP_Text text;


    private void Start()
    {
        text = GetComponent<TMP_Text>();
        SetLanguageText();
        LocalizationManager.Instance.OnLocalizationChange += SetLanguageText;
    }


    private void OnDestroy()
    {
        LocalizationManager.Instance.OnLocalizationChange -= SetLanguageText;
    }


    private void SetLanguageText()
    {
        string localizedText = LocalizationManager.Instance.GetLocalizedValue(key);
        if (localizedText == LocalizationManager.missingTextString) Debug.LogError($"{gameObject.name} has a missingTextString!");
        text.text = localizedText;
    }
}
