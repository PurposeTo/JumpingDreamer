using UnityEngine;
using UnityEngine.UI;

public class LanguageButton : MonoBehaviour {
    
    private Button button;

    private void Start () {
        button = GetComponent<Button>();
        button.onClick.AddListener(LocalizationManager.Instance.SetLanguage);
    }
}
