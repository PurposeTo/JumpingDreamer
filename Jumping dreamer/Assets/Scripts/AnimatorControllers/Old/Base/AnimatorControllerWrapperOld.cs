using UnityEngine;

//Должен быть SuperMonoBehaviour, так как аниматор не работает с выключеным объектом! (Можно ошибиться при работе с аниматором извне)

[RequireComponent(typeof(Animator))]
public abstract class AnimatorControllerWrapperOld : SuperMonoBehaviour
{
    private protected Animator animator;
    private protected virtual IAnimatorInitializerConfigs AnimatorInitializerConfigs { get; set; }


    protected override void AwakeWrapped()
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
