using UnityEngine;
using UnityEngine.UI;

namespace Desdiene.Localization
{
    public class LanguageButton : MonoBehaviour
    {

        private Button button;

        private void Start()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(LocalizationManager.Instance.SetLanguage);
        }
    }
}
