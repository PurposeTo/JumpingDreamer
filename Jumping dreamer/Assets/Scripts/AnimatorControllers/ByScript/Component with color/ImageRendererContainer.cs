using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageRendererContainer : ComponentWithColor
{
    private Image image;

    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
    }


    public override void ChangeColor(Color color)
    {
        image.color = color;
    }


    public override Color GetColor()
    {
        return image.color;
    }
}
