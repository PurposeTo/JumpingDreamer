using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class BasePlatform : MonoBehaviour
{
    private protected Rigidbody2D rb2D;
    private protected AnimatorBlinkingController animatorBlinkingController;

    private readonly float blinkingAnimationSpeed = 1.25f;

    private protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animatorBlinkingController = GetComponent<AnimatorBlinkingController>();
        animatorBlinkingController.SetBlinkingAnimationSpeed(blinkingAnimationSpeed);
        animatorBlinkingController.SetAnimationDuration(AnimatorBlinkingController.DurationType.Loops, 3);
        animatorBlinkingController.SetManualControl(manualControlEnableState: true, manualControlDisableState: false);
        animatorBlinkingController.OnDisableBlinking += DisableObject;
    }


    private void OnDestroy()
    {
        animatorBlinkingController.OnDisableBlinking -= DisableObject;
    }


    private void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
