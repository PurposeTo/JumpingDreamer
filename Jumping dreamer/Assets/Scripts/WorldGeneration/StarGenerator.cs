using Desdiene.ObjectPoolerAsset;
using Desdiene.Singleton;
using Desdiene.UnityEngineExtension;
using UnityEngine;

public class StarGenerator : SingletonSuperMonoBehaviour<StarGenerator>
{
    public GameObject Star;
    private float centreRadius;

    private readonly float delay = 10f;
    private float counter;

    public int NumberOfActiveStars { get; set; }


    private void Start()
    {
        centreRadius = Centre.CentreRadius;
    }


    private void Update()
    {
        if (counter > 0f)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            GenerateStar();
            counter = delay;
        }
    }


    private void GenerateStar()
    {
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        float range = Random.Range(centreRadius + 10f, 70f);
        Vector3 randomPosition = randomDirection * range;
        GameObject createdStar = Star.SpawnFromPool();
        createdStar.transform.position = randomPosition;
    }
}
