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
            ObjectPooler.Instance.SpawnFromPool(Flash, Vector2.zero, Quaternion.identity);
            yield return waitForSeconds;
        }
    }
}
