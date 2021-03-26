using System;
using Desdiene.Singleton;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Shutter : SingletonSuperMonoBehaviour<Shutter>
{
    private Animator animator;

    public event Action OnShutterOpen;
    public event Action OnShutterClose;

    //1.	Исходный скрипт – Вызываем метод “сменить уровень на Scene scene”
    //2.	Shutter - Игровое время останавливается
    //3.	Shutter - Заслонка закрывается
    //4.	Shutter - Заслонка закрылась.Загружается сцена, переданная в параметры метода “Закрыть заслонку”
    //5.	Shutter - Загружается нужная сцена
    //6.	Shutter - Заслонка открывается
    //7.	Shutter - Игровое время начинает идти

    protected override void AwakeSingleton()
    {
        animator = gameObject.GetComponent<Animator>();
    }


    public void CloseShutter()
    {
        animator.SetBool("isOpen", false);
    }


    public void OpenShutter()
    {
        animator.SetBool("isOpen", true);
    }


    // Этот метод вызывается аниматором после конца анимации открытия заслонки
    private void OnShutterOpenCall() => OnShutterOpen?.Invoke();


    // Этот метод вызывается аниматором после конца анимации закрытия заслонки
    private void OnShutterCloseCall() => OnShutterClose?.Invoke();
}
