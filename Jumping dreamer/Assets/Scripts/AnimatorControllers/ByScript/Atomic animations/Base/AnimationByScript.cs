using System;
using System.Collections;
using Desdiene.SuperMonoBehaviourAsset;
using UnityEngine;

/*
 * Анимация имеет:
 * Параметры выполнения
 *  - это поля, используемые во время выполнения анимации.
 * Параметры создания
 *  - это поля, используемые для создания анимации / анимационной кривой
 */

/// <summary>
/// Данный скрипт хранит дополняет анимацию параметрами.
/// Скрипты, наследуемые от данного класса должны описывать "Атомарную" анимацию
/// </summary>
public abstract class AnimationByScript : BaseAnimationByScript
{
    protected AnimationByScript(SuperMonoBehaviour superMonoBehaviour) : base(superMonoBehaviour)
    {
        SetBaseDefaultAnimationExecutionParameters();
        SetCommandsOnSwitchingActiveStateGameObject();
    }


    #region Параметры выполнения анимации
    protected bool unscaledTime = false;
    protected Func<float> GetDeltaTime { get; private set; }
    protected virtual float AnimationDuration { get; set; } = 1f; // Какая длительность у анимации? По умолчанию 1 сек?..

    #endregion


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
    /// Метод для установки параметров выполнения анимации по умолчанию.
    /// Вместе с данным методом произойдет инициализация DeltaTime (unscaled or not)
    /// </summary>
    protected virtual void SetDefaultAnimationExecutionParameters() { }


    private void SetBaseDefaultAnimationExecutionParameters()
    {
        SetTimeScaled(unscaledTime);
        SetDefaultAnimationExecutionParameters();
    }


    private void SetCommandsOnSwitchingActiveStateGameObject()
    {
        // При включении необходимо установить параметры анимации по умолчанию, которые были созданы при создании данного объекта.
        // Так же, необходимо установить в значения по умолчанию те параметры, которые анимация изменяла...
        superMonoBehaviour.OnEnabling += SetBaseDefaultAnimationExecutionParameters;
    }
}
