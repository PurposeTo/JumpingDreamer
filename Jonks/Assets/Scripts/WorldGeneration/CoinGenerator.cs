using System.Collections;
using UnityEngine;

public class CoinGenerator : SingletonMonoBehaviour<CoinGenerator>
{
    public GameObject Coin;
    private float centreRadius;

    private float delay = 10f;
    private float counter;

    public int NumberOfActiveCoins { get; set; }


    private void Start()
    {
        centreRadius = GameManager.Instance.CentreRadius;
    }


    private void Update()
    {
        if (counter > 0f)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            GenerateCoin();
            counter = delay;
        }
    }


    private void GenerateCoin()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float range = Random.Range(centreRadius * 1.5f, 60f);
        Vector3 randomPosition = randomDirection * range;

        ObjectPooler.Instance.SpawnFromPool(Coin, randomPosition, Quaternion.identity);
    }
}
