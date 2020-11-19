using UnityEngine;

public class SpriteRendererContainer : ComponentWithColor
{
    private SpriteRenderer spriteRenderer;

    protected override void AwakeWrapped()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }


    public override void SetColor(Color color)
    {
        spriteRenderer.color = color;
    }


    public override Color GetColor()
    {
        return spriteRenderer.color;
    }
}
