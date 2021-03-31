using System.Collections.Generic;
using Desdiene.SuperMonoBehaviourAsset;
using UnityEngine;

public abstract class ComponentWithColor : SuperMonoBehaviour
{
    public abstract void SetColor(Color color);

    public abstract Color GetColor();


    public void SetChangedAlphaChannelToColor(float newAlphaChannel)
    {
        Color cashedColor = GetColor();
        cashedColor.a = newAlphaChannel;
        SetColor(cashedColor);
    }
}
