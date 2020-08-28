using System;
using TMPro;
using UnityEngine;

public class ConfirmationOperationWindow : MonoBehaviour
{
    public TMP_InputField inputField;
    public TextMeshProUGUI confirmationOperationText;

    private string confirmationOperationKeyword;

    private string keywordKey;
    private string textKey;

    private Action<bool> onOperationConfirmed;


    private void Start()
    {
        LocalizationManager.Instance.OnLocalizationChange += SetLanguageText;
    }


    private void OnDestroy()
    {
        LocalizationManager.Instance.OnLocalizationChange -= SetLanguageText;
    }


    public void Initialize(string keyForConfirmationOperationKeyword, string keyForConfirmationOperationText, Action<bool> action)
    {
        keywordKey = keyForConfirmationOperationKeyword;
        textKey = keyForConfirmationOperationText;

        confirmationOperationKeyword = LocalizationManager.Instance.GetLocalizedValue(keyForConfirmationOperationKeyword);
        confirmationOperationText.text = string.Format(LocalizationManager.Instance.GetLocalizedValue(keyForConfirmationOperationText), confirmationOperationKeyword);
        onOperationConfirmed = action;
    }


    public void ConfirmDeletePlayerDataButton()
    {
        if (inputField.text == confirmationOperationKeyword) { onOperationConfirmed(true); }
        else { onOperationConfirmed(false); }

        CloseWindowButton();
    }


    public void CloseWindowButton()
    {
        inputField.text = string.Empty;
        gameObject.SetActive(false);
    }


    private void SetLanguageText()
    {
        confirmationOperationKeyword = LocalizationManager.Instance.GetLocalizedValue(keywordKey);
        confirmationOperationText.text = string.Format(LocalizationManager.Instance.GetLocalizedValue(textKey), confirmationOperationKeyword);
    }
}

