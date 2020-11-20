using System;
using System.Collections;
using UnityEngine;

/// <summary>
/// Данный скрипт хранит в себе анимационную кривую и методы для взаимодействия с ней.
/// </summary>
public abstract class BaseAnimationByScript : SuperMonoBehaviourContainer
{
    protected BaseAnimationByScript(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour)
    {
        SetBaseDefaultAnimationCreatingParameters();
    }


    protected AnimationCurve AnimationCurve { get; private set; }

    protected abstract AnimationCurve GetAnimationCurve();


    /// <summary>
    /// Вызывается при создании объекта.
    /// Метод для установки параметров создания анимации по умолчанию.
    /// После выполнения данного метода произойдет инициализация анимационной кривой, поэтому параметры можно задавать напрямую, 
    /// без использования ChangeAnimationCreatingParameters.
    /// </summary>
    protected abstract void SetDefaultAnimationCreatingParameters();


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


    public abstract IEnumerator AnimationEnumerator();


    private void SetBaseDefaultAnimationCreatingParameters()
    {
        SetDefaultAnimationCreatingParameters();
        SetAnimationCurve(GetAnimationCurve);
    }

    /// <summary>
    /// Метод для проверки - активен ли игровой объект.
    /// Идея такова, что на неактивном игровом объекте нельзя изменить параметры анимации, так как они сменятся на дефолтные при включении.
    /// </summary>
    private void CheckForGameObjectActive(Action onGameObjectActive)
    {
        if (superMonoBehaviour.gameObject.activeInHierarchy) onGameObjectActive?.Invoke();
        else Debug.LogWarning($"You can't change animation creating parameters in inactive gameObject!");
    }


    private void SetAnimationCurve(Func<AnimationCurve> GetAnimationCurve)
    {
        AnimationCurve = GetAnimationCurve();
    }
}

