using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteRendererContainer : ComponentWithColor
{
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }


    public override void ChangeColor(Color color)
    {
        spriteRenderer.color = color;
    }


    public override Color GetColor()
    {
        return spriteRenderer.color;
    }
}
