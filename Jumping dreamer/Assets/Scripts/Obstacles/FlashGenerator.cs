using UnityEngine;
using System.Collections;

public class FlashGenerator : MonoBehaviour
{
    [SerializeField] private GameObject Flash = null;

    private readonly float startDelay = 20f;
    private readonly float spawnFrequency = 5f;


    private void Start()
    {
        StartCoroutine(GenerateFlashEnumerator());
    }


    private IEnumerator GenerateFlashEnumerator()
    {
        yield return new WaitForSeconds(startDelay);

        WaitForSeconds waitForSeconds = new WaitForSeconds(spawnFrequency);
        while (true)
        {
            ObjectPooler.Instance.SpawnFromPool(Flash, Vector2.zero, Quaternion.identity);
            yield return waitForSeconds;
        }
    }
}
