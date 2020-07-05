using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class Shutter : Singleton<Shutter>
{
    private Animator animator;

    private string sceneToLoadName;

    //1.	Исходный скрипт – Вызываем метод “сменить уровень на Scene scene”
    //2.	Shutter - Игровое время останавливается
    //3.	Shutter - Заслонка закрывается
    //4.	Shutter - Заслонка закрылась.Загружается сцена, переданная в параметры метода “Закрыть заслонку”
    //5.	Shutter - Загружается нужная сцена
    //6.	Shutter - Заслонка открывается
    //7.	Shutter - Игровое время начинает идти

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }


    public void CloseShutterAndLoadScene(string sceneName)
    {
        sceneToLoadName = sceneName;

        Time.timeScale = 0f;
        animator.SetTrigger("Close");
    }


    // Этот метод вызывается после конца анимации закрытия заслонки
    public void LoadSceneAfterClosingShutter()
    {
        SceneManager.LoadScene(sceneToLoadName);
        animator.SetTrigger("Open");
    }


    // Этот метод вызывается после конца анимации открытия заслонки
    public void RecoverTimeAfterOpeningShutter()
    {
        sceneToLoadName = ""; // Необходимо очистить поле после загрузки сцены
        Time.timeScale = 1f;
    }
}
