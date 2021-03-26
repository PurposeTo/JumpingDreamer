using System;
using Desdiene.Super_monoBehaviour;
using UnityEngine;

public class TestAnimationByScript : SuperMonoBehaviour
{
    protected override void AwakeWrapped()
    {
        //new FadeAnimator(this, gameObject.GetComponent<SpriteRendererContainer>()).StartAnimation();
    }
}
