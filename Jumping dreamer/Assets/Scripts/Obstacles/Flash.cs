using UnityEngine;
using System.Collections;

public class Flash : MonoBehaviour, IPooledObject
{
    //private readonly float width = 5f;
    private readonly float lifetime = 4f;
    

    public void TurnOffFlash()
    {
        gameObject.SetActive(false);
    }


    private IEnumerator DestroyFlashEnumerator()
    {
        yield return new WaitForSeconds(lifetime);
    }


    void IPooledObject.OnObjectSpawn()
    {
        StartCoroutine(DestroyFlashEnumerator());
        TurnOffFlash();
    }
}

