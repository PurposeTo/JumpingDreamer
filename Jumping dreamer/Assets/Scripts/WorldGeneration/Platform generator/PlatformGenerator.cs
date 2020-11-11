using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public PlatformGeneratorConfigs PlatformGeneratorConfigs { get; private set; }
    private PlatformGeneratorData platformGeneratorData;

    private float counter = 0f;
    private protected List<Vector2> directionsAroundCircle = new List<Vector2>();


    private void Start()
    {
        GenerateRingFromVerticalMotionPlatforms();
    }


    private void Update()
    {
        Generating();
    }


    public void Constructor(PlatformGeneratorData platformGeneratorData)
    {
        if (platformGeneratorData == null) throw new System.ArgumentNullException("platformGeneratorData");
        this.platformGeneratorData = platformGeneratorData;
    }


    public void SetDefaultPlatformGenerationConfigs()
    {
        PlatformGeneratorConfigs = PlatformGeneratorConfigs.GetDefault();
    }


    public void SetNewPlatformGenerationConfigs()
    {
        PlatformGeneratorConfigs = PlatformGeneratorConfigs.GetRandom();
    }


    public void GenerateRingFromVerticalMotionPlatforms()
    {
        GenerateRingFromPlatforms(platformGeneratorData.VerticalMotionPlatform, 20f, 5f);
    }


    private void Generating()
    {
        if (counter > 0f)
        {
            counter -= Time.deltaTime;
        }
        else
        {
            CheckDirectionsAroundCircle();
            GeneratePlatform();

            counter = PlatformGeneratorConfigs.TimePeriodForGeneratingPlatforms;
        }
    }


    private void CheckDirectionsAroundCircle()
    {
        if (directionsAroundCircle.Count == 0)
        {
            // Расстояние между точками - 5 градусов
            Vector2[] vector2sDirectionsArray = GameLogic.GetVector2sDirectionsAroundCircle(5f);
            GameLogic.Shuffle(vector2sDirectionsArray);
            directionsAroundCircle = vector2sDirectionsArray.ToList();
            Debug.Log("Update directionsAroundCircle array in platform generator");
        }
    }


    private void GeneratePlatform()
    {
        PlatformConfigs platformConfigs = PlatformGeneratorConfigs.PlatformConfigs;

        // Позиция равна первому элементу в списке. После использования позиции, убрать из списка
        Vector3 position = GameObjectsHolder.Instance.Centre.gameObject.transform.position + (Vector3)directionsAroundCircle[0];
        directionsAroundCircle.RemoveAt(0);
        position *= platformGeneratorData.GetCreatingRange(platformConfigs.CreatingPlace);

        GameObject platformToCreate = platformGeneratorData.GetPlatform(platformConfigs.MovingTypes);
        GameObject createdPlatform = ObjectPooler.Instance.SpawnFromPool(platformToCreate, position, Quaternion.identity);
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
