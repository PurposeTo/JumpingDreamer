using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class TMProRendererContainer : ComponentWithColor
{
    private TextMeshPro textMeshPro;

    private void Awake()
    {
        textMeshPro = gameObject.GetComponent<TextMeshPro>();
    }


    public override void ChangeColor(Color color)
    {
        textMeshPro.color = color;
    }


    public override Color GetColor()
    {
        return textMeshPro.color;
    }
}
