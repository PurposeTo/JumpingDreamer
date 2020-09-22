using UnityEngine;

public class AnimatorControllerWrapper : MonoBehaviour
{
    private protected Animator animator;

    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
    }
}
