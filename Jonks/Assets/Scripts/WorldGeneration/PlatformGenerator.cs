using UnityEngine;

public class PlatformGenerator : SingletonMonoBehaviour<PlatformGenerator>
{
    public GameObject DefaultPlatform;

    private float delay = 0.5f;
    private float counter;


    private void Start()
    {
        //GenerateRingFromPlatforms(DefaultPlatform, 15f, 5f);
        GenerateRingFromPlatforms(DefaultPlatform, 20f, 5f);

    }


    private void Update()
    {
        GeneratePlatforms(DefaultPlatform);
    }


    /// <summary>
    /// Генерирует кольцо из платформ, вокруг Центра
    /// </summary>
    /// <param name="platform">Тип платформы, которая будет сгенерирована</param>
    /// <param name="distanceAngle">Угол, на расстоянии которого будут распологаться платформы</param>
    /// <param name="range">Расстояние от поверхности Центра до платформы</param>
    private void GenerateRingFromPlatforms(GameObject platform, float distanceAngle, float range)
    {
        range += GameManager.Instance.CentreRadius;

        int platformsCount = (int)(360f / distanceAngle);

        for (int i = 0; i < platformsCount; i++)
        {
            float rotateAngle = distanceAngle * i;
            Quaternion rotation = Quaternion.Euler(0, 0, rotateAngle);
            Vector2 direction = rotation * Vector3.up;
            Vector3 position = direction * range;

            ObjectPooler.Instance.SpawnFromPool(platform, position, Quaternion.identity);
        }
    }


    private void GeneratePlatforms(GameObject platform)
    {
        if (counter > 0f)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            ObjectPooler.Instance.SpawnFromPool(platform, GameManager.Instance.Centre.transform.position, Quaternion.identity);
            counter = delay;
        }
    }
}
