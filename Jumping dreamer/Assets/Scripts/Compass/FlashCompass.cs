using UnityEngine;
using System.Collections;

public class FlashCompass : MonoBehaviour, IPooledObject
{
    private float transparency = 1f;
    private readonly float blinkingRate = 30f;

    private SpriteRenderer sprite;

    private Coroutine lifeCycleRoutine;


    private void Initialize()
    {
        sprite = gameObject.GetComponent<SpriteRenderer>();
    }


    private IEnumerator LifeCycleEnumerator()
    {
        yield return new WaitForSeconds(Flash.FlashStartDelay);
        gameObject.SetActive(false);

        lifeCycleRoutine = null;
    }


    void IPooledObject.OnObjectSpawn()
    {
        Initialize();
        if (lifeCycleRoutine == null) StartCoroutine(LifeCycleEnumerator());
    }
}
