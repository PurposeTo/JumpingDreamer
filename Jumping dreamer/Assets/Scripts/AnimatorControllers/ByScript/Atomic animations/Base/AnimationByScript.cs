using System;
using System.Collections;
using UnityEngine;

/*
 * Анимация имеет:
 * Настройки
 *  - это...
 * Параметры выполнения
 *  - это поля, используемые во время выполнения анимации.
 * Параметры создания
 *  - это поля, используемые для создания анимации / анимационной кривой
 * 
 */

/// <summary>
/// Скрипты, наследуемые от данного класса должны описывать "Атомарную" анимацию
/// </summary>
public abstract class AnimationByScript : SuperMonoBehaviourContainer, IAnimationExecutable
{
    protected AnimationByScript(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour)
    {
        animationInfo = superMonoBehaviour.CreateCoroutineInfo();
        SetBaseDefaultAnimationCreatingParameters();
        SetBaseDefaultAnimationExecutionParameters();
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

    #region Параметры выполнения анимации
    protected bool unscaledTime = false;
    protected Func<float> GetDeltaTime;
    protected virtual float AnimationDuration { get; set; } = 1f; // Какая длительность у анимации? По умолчанию 1 сек?..

    #endregion

    protected AnimationCurve AnimationCurve { get; private set; }

    private ICoroutineInfo animationInfo;


    public void StartAnimation()
    {
        superMonoBehaviour.ExecuteCoroutineContinuously(ref animationInfo, AnimationEnumeratorSuper());
    }


    public void BreakAnimation()
    {
        superMonoBehaviour.BreakCoroutine(ref animationInfo);
    }


    public void SetAnimationDuration(float animationDuration)
    {
        ChangeAnimationExecutionParameters(() => AnimationDuration = animationDuration);
    }


    public void SetTimeScaled(bool unscaledTime)
    {
        ChangeAnimationExecutionParameters(() =>
        {
            this.unscaledTime = unscaledTime;
            if (unscaledTime) GetDeltaTime = () => Time.unscaledDeltaTime;
            else GetDeltaTime = () => Time.deltaTime;
        });
    }


    /// <summary>
    /// Вызывается при создании объекта.
    /// Метод для установки параметров создания анимации по умолчанию.
    /// После выполнения данного метода произойдет инициализация анимационной кривой, поэтому параметры можно задавать напрямую, 
    /// без использования ChangeAnimationCreatingParameters.
    /// </summary>
    protected virtual void SetDefaultAnimationCreatingParameters() { }

    /// <summary>
    /// Вызывается при создании объекта.
    /// Метод для установки параметров выполнения анимации по умолчанию.
    /// Вместе с данным методом произойдет инициализация DeltaTime (unscaled or not)
    /// </summary>
    protected virtual void SetDefaultAnimationExecutionParameters() { }


    /// <summary>
    /// Метод для изменения параметров выполнения анимации
    /// Использует проверку на активность игрового объекта.
    /// </summary>
    /// <param name="changeAnimationExecutionParameters"></param>
    protected void ChangeAnimationExecutionParameters(Action changeAnimationExecutionParameters)
    {
        CheckForGameObjectActive(() =>
        {
            changeAnimationExecutionParameters?.Invoke();
        });
    }

    /// <summary>
    /// Метод для изменения параметров создания анимации. В конце выполнения заново проинициализирует анимационную кривую.
    /// Использует проверку на активность игрового объекта.
    /// </summary>
    /// <param name="changeAnimationCreatingParameters">Методы, изменяющие параметры создания анимации.</param>
    protected void ChangeAnimationCreatingParameters(Action changeAnimationCreatingParameters)
    {
        CheckForGameObjectActive(() =>
        {
            changeAnimationCreatingParameters?.Invoke();
            SetAnimationCurve(GetAnimationCurve);
        });
    }


    protected abstract AnimationCurve GetAnimationCurve();


    protected abstract IEnumerator AnimationEnumerator();


    private void SetBaseDefaultAnimationCreatingParameters()
    {
        SetDefaultAnimationCreatingParameters();
        SetAnimationCurve(GetAnimationCurve);
    }


    private void SetBaseDefaultAnimationExecutionParameters()
    {
        SetTimeScaled(unscaledTime);
        SetDefaultAnimationExecutionParameters();
    }


    /// <summary>
    /// Метод для проверки - активен ли игровой объект.
    /// Идея такова, что на неактивном игровом объекте нельзя изменить параметры анимации, так как они сменятся на дефолтные при включении.
    /// </summary>
    private void CheckForGameObjectActive(Action onGameObjectActive)
    {
        if (superMonoBehaviour.gameObject.activeInHierarchy)
        {
            onGameObjectActive?.Invoke();
        }
        else
        {
            Debug.LogWarning($"You can't change animation creating parameters in inactive gameObject!");
        }
    }


    private void SetCommandsOnSwitchingActiveStateGameObject()
    {
        // При включении необходимо установить параметры анимации по умолчанию, которые были созданы при создании данного объекта.
        // Так же, необходимо установить в значения по умолчанию те параметры, которые анимация изменяла...
        superMonoBehaviour.OnEnabling += SetBaseDefaultAnimationExecutionParameters;


        //При выключении необходимо проверить, работает ли анимация. Если да, то кинуть варнинг.
        superMonoBehaviour.OnDisabling += () =>
        {
            if (animationInfo.IsExecuting)
            {
                superMonoBehaviour.BreakCoroutine(ref animationInfo);
                Debug.Log($"{superMonoBehaviour} is disabling, but {GetType()} is executing! Breaking the animation...");
            }
        };
    }


    private IEnumerator AnimationEnumeratorSuper()
    {
        yield return AnimationEnumerator();
        OnAnimationEnd?.Invoke();
    }


    private void SetAnimationCurve(Func<AnimationCurve> GetAnimationCurve)
    {
        AnimationCurve = GetAnimationCurve();
    }
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
