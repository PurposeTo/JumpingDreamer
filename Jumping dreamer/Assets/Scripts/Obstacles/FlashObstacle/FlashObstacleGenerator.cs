using UnityEngine;
using System.Collections;

public class FlashObstacleGenerator : MonoBehaviour
{
    [SerializeField] private GameObject Flash = null;
    [SerializeField] private GameObject flashCompass = null;

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
            GameObject flash = ObjectPooler.Instance.SpawnFromPool(Flash, Vector2.zero, Quaternion.identity);
            flashCompass.SetActive(true);
            flashCompass.GetComponent<FlashCompass>().Constructor(flash.GetComponent<Flash>());

            yield return waitForSeconds;
        }
    }
}
