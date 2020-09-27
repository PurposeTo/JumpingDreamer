using UnityEngine;

public abstract class AnimatorControllerWrapper : MonoBehaviour
{
    private protected Animator animator;
    private protected virtual IAnimatorInitializerConfigs AnimatorInitializerConfigs { get; set; }


    private void Awake()
    {
        animator = gameObject.GetComponent<Animator>();
        AnimatorInitializerConfigs.SetAnimator(animator);
        AnimatorInitializerConfigs.InitializeConfigs();
        AnimatorInitializerConfigs.OnConfigsChanged += AnimatorInitializerConfigs.InitializeConfigs;
    }

    private void OnDestroy()
    {
        AnimatorInitializerConfigs.OnConfigsChanged -= AnimatorInitializerConfigs.InitializeConfigs;
    }
}
