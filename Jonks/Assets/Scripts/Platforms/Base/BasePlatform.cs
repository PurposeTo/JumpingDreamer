using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BasePlatform : MonoBehaviour
{
    private protected GameObject centre;
    private protected Rigidbody2D rb2D;
    private protected AnimatorBlinkingController animatorBlinkingController;

    private float blinkingAnimationSpeed = 1.25f;

    private protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        animatorBlinkingController = GetComponent<AnimatorBlinkingController>();
        animatorBlinkingController.SetBlinkingAnimationSpeed(blinkingAnimationSpeed);
        animatorBlinkingController.SetAnimationDuration(AnimatorBlinkingController.DurationType.Loops, 3);
        animatorBlinkingController.OnDisableBlinking += DisableObject;
    }

    private protected virtual void Start()
    {
        centre = GameManager.Instance.CentreObject;
    }


    private void OnDestroy()
    {
        animatorBlinkingController.OnDisableBlinking -= DisableObject;
    }


    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
