using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Desdiene.Localization
{

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
            text.text = LocalizationManager.Instance.GetLocalizedValue(key);
        }
    }
}
