using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour, IPooledObject
{
    [SerializeField] private GameObject killingZoneObject;

    //private readonly float width = 5f;
    private readonly float startDelay = 1.5f;
    private readonly float flashOperatingTime = 1.5f;

    private Coroutine lifeCycleRoutine;


    private void OnDisable()
    {
        RepairFlash();
    }


    // Для аниматора
    private void TurnOffFlash()
    {
        gameObject.SetActive(false);
    }


    // Перед выключением вернуть объект вспышки в исходное состояние
    private void RepairFlash()
    {
        killingZoneObject.SetActive(false);
    }


    private IEnumerator LifeCycleEnumerator()
    {
        yield return new WaitForSeconds(startDelay);
        killingZoneObject.SetActive(true); // Включение дочернего объекта
        yield return new WaitForSeconds(flashOperatingTime);
        TurnOffFlash();

        lifeCycleRoutine = null;
    }


    void IPooledObject.OnObjectSpawn()
    {
        if (lifeCycleRoutine == null) lifeCycleRoutine = StartCoroutine(LifeCycleEnumerator());
    }
}

