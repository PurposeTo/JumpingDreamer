using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlatformGeneratorData))]
public class PlatformGeneratorController : SingletonMonoBehaviour<PlatformGeneratorController>
{
    private PlatformGeneratorData platformGeneratorData;
    public PlatformGenerator PlatformGenerator { get; private set; }

    private readonly float timePeriodForTheGenerationRules = 30f;
    private Coroutine lifeCycleRoutine;


    private void Start()
    {
        platformGeneratorData = gameObject.GetComponent<PlatformGeneratorData>();
        PlatformGenerator = new PlatformGenerator(platformGeneratorData);
        if (lifeCycleRoutine == null) StartCoroutine(LifeCycleEnumerator());
    }


    private void Update()
    {
        PlatformGenerator.Generating();
    }


    private IEnumerator LifeCycleEnumerator()
    {
        GenerateRingFromPlatforms(platformGeneratorData.VerticalMotionPlatform, 20f, 5f);

        while (true)
        {
            yield return new WaitForSeconds(timePeriodForTheGenerationRules);
            PlatformGenerator.SetNewPlatformGeneratorConfigs();
        }

        lifeCycleRoutine = null;
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
