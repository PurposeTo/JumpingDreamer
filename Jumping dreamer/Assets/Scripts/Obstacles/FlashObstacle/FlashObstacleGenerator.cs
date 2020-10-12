using UnityEngine;
using System.Collections;

public class FlashObstacleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject flash = null;
    [SerializeField] private GameObject flashCompass = null;
    [SerializeField] private RectTransform flashCompassCanvas = null;

    private readonly float startDelay = 20f;
    private readonly float spawnFrequency = 5f;


    private void Start()
    {
        StartCoroutine(LifeCycleEnumerator());
    }


    private IEnumerator LifeCycleEnumerator()
    {
        yield return new WaitForSeconds(startDelay);

        WaitForSeconds waitForSeconds = new WaitForSeconds(spawnFrequency);
        while (true)
        {
            GameObject createdFlash = ObjectPooler.Instance.SpawnFromPool(flash, Vector2.zero, Quaternion.identity);
            GameObject compass = ObjectPooler.Instance.SpawnFromPool(flashCompass, Vector2.zero, Quaternion.identity, flashCompassCanvas.transform);
            compass.GetComponent<FlashCompass>().Constructor(createdFlash.GetComponent<Flash>());

            yield return waitForSeconds;
        }
    }
}
