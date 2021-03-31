using System;
using System.Collections;
using Desdiene.Container;
using Desdiene.Coroutine.CoroutineExecutor;
using Desdiene.SuperMonoBehaviourAsset;
using UnityEngine;

public class AnimatorByScript<T> : SuperMonoBehaviourContainer where T : AnimationByScript
{
    public T Animation { get; private set; }

    public AnimatorByScript(T animationByScript, SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour)
    {
        this.Animation = animationByScript ?? throw new ArgumentNullException(nameof(animationByScript));
        animationInfo = superMonoBehaviour.CreateCoroutineContainer();
        SetCommandsOnSwitchingActiveStateGameObject();
    }

    /// <summary>
    /// Выполняется по завершении анимации
    /// </summary>
    public event Action OnAnimationEnd;

    /// <summary>
    /// Возвращает bool значение - выполняется ли в данный момент анимация?
    /// </summary>
    public bool IsExecuting => animationInfo.IsExecuting;



    private ICoroutineContainer animationInfo;


    public void StartAnimation()
    {
        superMonoBehaviour.ExecuteCoroutineContinuously(animationInfo, AnimationEnumeratorSuper());
    }


    public void BreakAnimation()
    {
        superMonoBehaviour.BreakCoroutine(animationInfo);
    }


    private IEnumerator AnimationEnumeratorSuper()
    {
        yield return Animation.AnimationEnumerator();
        OnAnimationEnd?.Invoke();
    }


    private void SetCommandsOnSwitchingActiveStateGameObject()
    {
        //При выключении необходимо проверить, работает ли анимация. Если да, то кинуть остановить и сообщить об этом.
        superMonoBehaviour.OnDisabling += () =>
        {
            if (animationInfo.IsExecuting)
            {
                superMonoBehaviour.BreakCoroutine(animationInfo);
                Debug.Log($"{superMonoBehaviour} is disabling, but {GetType()} is executing! Breaking the animation...");
            }
        };
    }

}
