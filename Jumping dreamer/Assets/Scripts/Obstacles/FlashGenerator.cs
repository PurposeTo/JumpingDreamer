using UnityEngine;
using System.Collections;

public class FlashGenerator : MonoBehaviour
{
    [SerializeField] private GameObject Flash = null;

    private float spawnFrequency = 4f;
    private Vector2 direction;


    private void Start()
    {
        StartCoroutine(GenerateFlashEnumerator());
    }


    private IEnumerator GenerateFlashEnumerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnFrequency);
            direction = Random.insideUnitCircle.normalized * Centre.CentreRadius;
            ObjectPooler.Instance.SpawnFromPool(Flash, direction, Quaternion.identity);
        }
    }
}
