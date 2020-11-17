using UnityEngine;

public abstract class ComponentWithColor : MonoBehaviour
{
    public abstract void ChangeColor(Color color);

    public abstract Color GetColor();


    public void SetChangedAlphaChannelToColor(float newAlphaChannel)
    {
        Color cashedColor = GetColor();
        cashedColor.a = newAlphaChannel;
        ChangeColor(cashedColor);
    }
}
