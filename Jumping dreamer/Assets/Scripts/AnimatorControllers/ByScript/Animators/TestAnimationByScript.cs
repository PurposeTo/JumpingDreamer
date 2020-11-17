using System;
using UnityEngine;

public class TestAnimationByScript : SuperMonoBehaviour
{
    protected override void AwakeWrapped()
    {
        new FadeAnimator(this, gameObject.GetComponent<SpriteRendererContainer>()).StartAnimation();
    }
}
