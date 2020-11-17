using System;
using UnityEngine;


/// <summary>
/// Данный интерфейс создан для того, что бы избежать конфликтов при инициализации Animator-а. 
/// Классы, реализующие данный интерфейс, задают инициализирующие настройки аниматора, а AnimatorControllerWrapper их применяет.
/// </summary>
public interface IAnimatorInitializerConfigs
{
    void SetAnimator(Animator animator);

    event Action OnConfigsChanged;
    void InitializeConfigs();
}
