using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour, IPooledObject
{
    //private readonly float width = 5f;
    private readonly float lifetime = 4f;


    private IEnumerator DestroyFlashEnumerator()
    {
        yield return new WaitForSeconds(lifetime);
        gameObject.SetActive(false);
    }


    void IPooledObject.OnObjectSpawn()
    {
        StartCoroutine(DestroyFlashEnumerator());
    }
}

