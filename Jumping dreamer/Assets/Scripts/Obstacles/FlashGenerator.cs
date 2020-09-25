using UnityEngine;
using System.Collections;

public class FlashGenerator : MonoBehaviour
{
    [SerializeField] private GameObject Flash = null;

    private readonly float spawnFrequency = 4f;


    private void Start()
    {
        StartCoroutine(GenerateFlashEnumerator());
    }


    private IEnumerator GenerateFlashEnumerator()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(spawnFrequency);

        while (true)
        {
            Vector2 direction = Random.insideUnitCircle.normalized * Centre.CentreRadius;
            ObjectPooler.Instance.SpawnFromPool(Flash, direction, Quaternion.identity);
            yield return waitForSeconds;
        }
    }
}
