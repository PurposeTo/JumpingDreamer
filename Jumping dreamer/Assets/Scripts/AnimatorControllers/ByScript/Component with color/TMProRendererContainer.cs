using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TMProRendererContainer : ComponentWithColor
{
    private TMP_Text textMeshPro;

    private void Awake()
    {
        textMeshPro = gameObject.GetComponent<TMP_Text>();
    }


    public override void SetColor(Color color)
    {
        textMeshPro.color = color;
    }


    public override Color GetColor()
    {
        return textMeshPro.color;
    }
}
