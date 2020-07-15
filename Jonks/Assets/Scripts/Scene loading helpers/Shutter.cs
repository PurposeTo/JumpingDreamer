using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Animator))]
public class Shutter : SingletonMonoBehaviour<Shutter>
{
    private Animator animator;

    private string sceneToLoadName;

    private Coroutine waitingDataLoadRoutine;

    //1.	Исходный скрипт – Вызываем метод “сменить уровень на Scene scene”
    //2.	Shutter - Игровое время останавливается
    //3.	Shutter - Заслонка закрывается
    //4.	Shutter - Заслонка закрылась.Загружается сцена, переданная в параметры метода “Закрыть заслонку”
    //5.	Shutter - Загружается нужная сцена
    //6.	Shutter - Заслонка открывается
    //7.	Shutter - Игровое время начинает идти

    protected override void AwakeSingleton()
    {
        base.AwakeSingleton();
        animator = gameObject.GetComponent<Animator>();
        SceneManager.sceneLoaded += OnSceneLoaded;
        Time.timeScale = 0f;
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"OnSceneLoaded: {scene.name} in mode: {mode}");

        if (waitingDataLoadRoutine == null)
        {
            StartCoroutine(WaitingDataLoadEnumerator());
        }
        else
        {
            Debug.LogError("Error! WaitingDataLoadRoutine is already working!");
        }
    }


    public void CloseShutterAndLoadScene(string sceneName)
    {
        sceneToLoadName = sceneName;

        Time.timeScale = 0f;
        animator.SetBool("isOpen", false);
    }


    // Этот метод вызывается после конца анимации закрытия заслонки
    public void LoadSceneAfterClosingShutter()
    {
        SceneManager.LoadScene(sceneToLoadName);
    }

    // Запуск сцены или первое включение игры.
    // Заслонка закрыта. Ее необходимо открыть только тогда, когда будут загружены данные
    public void OpenShutter()
    {
        animator.SetBool("isOpen", true);
    }


    // Этот метод вызывается после конца анимации открытия заслонки
    public void RecoverTimeAfterOpeningShutter()
    {
        sceneToLoadName = ""; // Необходимо очистить поле после загрузки сцены
        Time.timeScale = 1f;
    }


    private IEnumerator WaitingDataLoadEnumerator()
    {
        // Когда DataLoaderController будет доработан, просто убрать раскомментировать строки и убрать yield break;

        //if (DataLoaderController.isReady())
        //{
        OpenShutter();
        //}
        //else
        //{
        //  yield return null;
        //}

        waitingDataLoadRoutine = null;
        yield break;
    }
}
