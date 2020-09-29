using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour, IPooledObject
{
    public static float FlashStartDelay => 2f;

    [SerializeField] private GameObject killingZoneObject;

    //private readonly float width = 5f;
    private Vector2 direction;
    private readonly float flashOperatingTime = 1.5f;

    private Coroutine lifeCycleRoutine;


    private void OnDisable()
    {
        RepairFlash();
    }


    private void InitializeFlashDirection()
    {
        direction = Random.insideUnitCircle.normalized * Centre.CentreRadius;
        transform.position = direction;
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
        InitializeFlashDirection();
        FlashCompassGenerator.Instance.GenerateFlashCompass(direction);
        yield return new WaitForSeconds(FlashStartDelay);

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

