using System;
using System.Collections;
using UnityEngine;

/*
 * 1. Данный скрипт должен останавливать анимацию и сбрасывать все настройки при выключении объекта.
 * 2. Если объект выключается до того, как анимация закончилась, необходимо кинуть варнинг, что так делать нельзя
 */

/// <summary>
/// Скрипты, наследуемые от данного класса должны описывать "Атомарную" анимацию
/// </summary>
public abstract class AnimationByScript : SuperMonoBehaviourContainer, IAnimationExecutable
{
    protected AnimationByScript(SuperMonoBehaviour superMonoBehaviour)
        : base(superMonoBehaviour)
    {
        animationInfo = superMonoBehaviour.CreateCoroutineInfo();
        SetDefaultAnimationConfigs();
        SetDefaultAnimationParameters();
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

    #region Настройки анимации
    protected bool unscaledTime = false;
    protected float deltaTime;

    #endregion


    #region Параметры анимации
    protected virtual float AnimationDuration { get; set; } = 1f; // Какая длительность у анимации? По умолчанию 1 сек?..

    #endregion

    protected AnimationCurve animationCurve;

    private ICoroutineInfo animationInfo;

    public void StartAnimation()
    {
        superMonoBehaviour.ContiniousCoroutineExecution(ref animationInfo, AnimationEnumeratorSuper());
    }


    public void BreakAnimation()
    {
        superMonoBehaviour.BreakCoroutine(ref animationInfo);
    }

    public virtual void SetDefaultAnimationConfigs()
    {
        SetTimeScaled(unscaledTime);
        SetAnimationCurve();
    }

    public abstract void SetDefaultAnimationParameters();


    public void SetAnimationDuration(float animationDuration) => AnimationDuration = animationDuration;


    public void SetTimeScaled(bool unscaledTime)
    {
        this.unscaledTime = unscaledTime;

        deltaTime = unscaledTime
            ? Time.unscaledDeltaTime
            : Time.deltaTime;
    }


    private void SetCommandsOnSwitchingActiveStateGameObject()
    {
        // При включении необходимо установить параметры анимации по умолчанию, которые были созданы при создании данного объекта.
        //superMonoBehaviour.OnEnabling += () => SetDefaultAnimationParameters();


        // При выключении необходимо проверить, работает ли анимация. Если да, то кинуть варнинг.
        //superMonoBehaviour.OnDisabling += () => 
        //if (animationInfo.IsExecuting)
        //{
        //    superMonoBehaviour.BreakCoroutine(ref animationInfo);
        //    Debug.LogWarning($"{nameof(superMonoBehaviour)} is disabling, but {this.GetType()} is executing! Breaking the animation...");
        //}
    }


    private IEnumerator AnimationEnumeratorSuper()
    {
        yield return AnimationEnumerator();
        OnAnimationEnd?.Invoke();
    }

    protected abstract IEnumerator AnimationEnumerator();

    protected abstract void SetAnimationCurve();
}



public interface IAnimationExecutable
{
    /// <summary>
    /// Выполняется по завершении анимации
    /// </summary>
    event Action OnAnimationEnd;

    /// <summary>
    /// Возвращает bool значение - выполняется ли в данный момент анимация?
    /// </summary>
    bool IsExecuting { get; }

    /// <summary>
    /// Запустить анимацию
    /// </summary>
    void StartAnimation();

    /*Если используются сложные переходы состояний / ожидания неких условий, 
     * что бы продолжить выполнение анимации, то стоит использовать IsExecuting.
     * 
     * А если необходимо сразу испольнить некие инструкции после окончания выполнения анимации, 
     * то лучше использовать OnAnimationEnd.
     * 
     * Так же, стоит учитывать, 
     * что при использовании ожидании через IsExecuting след. инструкции будут выполняться только в след. кадре.
     */
}
