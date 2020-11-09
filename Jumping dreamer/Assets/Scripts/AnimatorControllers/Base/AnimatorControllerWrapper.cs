﻿using UnityEngine;

public abstract class AnimatorControllerWrapper : SuperMonoBehaviour
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
