using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour, IPooledObject
{
    public Vector2 Direction { get; private set; }
    public bool IsFlashKillingZoneActive { get; private set; } = false;


    [SerializeField] private GameObject killingZoneObject = null;

    //private readonly float width = 5f;
    private readonly float flashStartDelay = 2f;
    private readonly float flashOperatingTime = 1.5f;

    private Coroutine lifeCycleRoutine;


    private void OnDisable()
    {
        RepairFlash();
    }


    private void InitializeFlashDirection()
    {
        Direction = Random.insideUnitCircle.normalized * Centre.CentreRadius;
        //Direction = Vector2.left * Centre.CentreRadius; // Для проверки работы добавленной логики
        transform.position = Direction;
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
        IsFlashKillingZoneActive = false;
    }


    private IEnumerator LifeCycleEnumerator()
    {
        InitializeFlashDirection();
        yield return new WaitForSeconds(flashStartDelay);

        killingZoneObject.SetActive(true); // Включение дочернего объекта
        IsFlashKillingZoneActive = true;
        yield return new WaitForSeconds(flashOperatingTime);

        TurnOffFlash();

        lifeCycleRoutine = null;
    }


    void IPooledObject.OnObjectSpawn()
    {
        if (lifeCycleRoutine == null) lifeCycleRoutine = StartCoroutine(LifeCycleEnumerator());
    }
}

