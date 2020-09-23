using UnityEngine;

public class PlatformGeneratorController : SingletonMonoBehaviour<PlatformGeneratorController>
{
    private PlatformGeneratorData platformGeneratorData;

    private PlatformGeneratorState platformGeneratorState;

    private void Start()
    {
        platformGeneratorData = gameObject.GetComponent<PlatformGeneratorData>();
        //platformGeneratorState = platformGeneratorData.DefaultGeneratorState;
        platformGeneratorState = platformGeneratorData.DefaultGeneratorState;
        GenerateRingFromPlatforms(platformGeneratorData.DefaultPlatform, 20f, 5f);
    }


    private void Update()
    {
        platformGeneratorState.Generating();
    }


    /// <summary>
    /// Генерирует кольцо из платформ, вокруг Центра
    /// </summary>
    /// <param name="platform">Тип платформы, которая будет сгенерирована</param>
    /// <param name="distanceAngle">Угол, на расстоянии которого будут распологаться платформы</param>
    /// <param name="range">Расстояние от поверхности Центра до платформы</param>
    private void GenerateRingFromPlatforms(GameObject platform, float distanceAngle, float range)
    {
        range += Centre.CentreRadius;

        Vector2[] vector2sDirections = GameLogic.GetVector2sDirectionsAroundCircle(distanceAngle);

        foreach (Vector2 direction in vector2sDirections)
        {
            Vector3 position = direction * range;
            ObjectPooler.Instance.SpawnFromPool(platform, position, Quaternion.identity);
        }
    }
}
