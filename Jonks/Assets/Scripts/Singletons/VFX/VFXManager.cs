using TMPro;
using UnityEngine;

public class VFXManager : Singleton<VFXManager>
{
    [SerializeField] private GameObject PopupTextPrefab;

    public void DisplayPopupText(Vector3 position, string text, float fontSize = 4f)
    {
        Color defaultColor = Color.white;

        DisplayPopupText(position, Quaternion.identity, text, defaultColor, fontSize);
    }


    public void DisplayPopupText(Vector3 position, Quaternion rotation, string text, Color color, float fontSize = 4f)
    {
        TextMeshPro textScript = SpawnPopupText(position, rotation);

        textScript.fontSize = fontSize;
        textScript.color = color;
        textScript.text = text;
    }


    private TextMeshPro SpawnPopupText(Vector3 position, Quaternion rotation)
    {
        GameObject PopupTextObject = ObjectPooler.Instance.SpawnFromPool(PopupTextPrefab, position, rotation);
        return PopupTextObject.GetComponent<TextMeshPro>();
    }
}
