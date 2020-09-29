using UnityEngine;

public class FlashCompassGenerator : SingletonMonoBehaviour<FlashCompassGenerator>
{
    [SerializeField] private GameObject FlashCompass = null;


    public void GenerateFlashCompass(Vector3 compassDirection)
    {
        ObjectPooler.Instance.SpawnFromPool(FlashCompass, compassDirection, Quaternion.identity);
    }
}

