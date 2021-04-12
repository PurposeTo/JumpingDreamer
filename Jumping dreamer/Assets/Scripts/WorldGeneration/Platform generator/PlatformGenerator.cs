using System.Collections.Generic;
using System.Linq;
using Desdiene.ObjectPoolerAsset;
using Desdiene;
using Desdiene.SuperMonoBehaviourAsset;
using UnityEngine;

public class PlatformGenerator : SuperMonoBehaviour
{
    public PlatformGeneratorConfigs PlatformGeneratorConfigs { get; private set; }
    private PlatformGeneratorData platformGeneratorData;

    private float counter = 0f;
    private protected List<Vector2> directionsAroundCircle = new List<Vector2>();


    protected override void StartWrapped()
    {
        GenerateRingFromVerticalMotionPlatforms();
    }


    protected override void UpdateWrapped()
    {
        Generating();
    }


    public void Constructor(PlatformGeneratorData platformGeneratorData)
    {
        this.platformGeneratorData = platformGeneratorData != null ? platformGeneratorData : throw new System.ArgumentNullException("platformGeneratorData");
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
            Randomizer.Shuffle(vector2sDirectionsArray);
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

        SpawnPlatform(platformToCreate, position);
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
            SpawnPlatform(platform, position);
        }
    }


    private GameObject SpawnPlatform(GameObject prefabPlatform, Vector3 position)
    {
        GameObject createdPlatform = prefabPlatform.SpawnFromPool();
        createdPlatform.transform.position = position;
        SetMotionConfigsAndGetCauseOfDestroy(createdPlatform, out PlatformCauseOfDestroy.CauseOfDestroy causeOfDestroy);
        createdPlatform.GetComponent<PlatformLifeCycle>().SetCauseOfDestroy(causeOfDestroy);
        return createdPlatform;
    }


    private void SetMotionConfigsAndGetCauseOfDestroy(GameObject createdPlatform, out PlatformCauseOfDestroy.CauseOfDestroy platformCausesOfDestroy)
    {
        // CauseOfDestroy в некоторых случаях может задаваться не при общей настройке правил генерации, а при определении правил движения вертикальной платформы.


        var movingTypeConfigs = PlatformGeneratorConfigs.PlatformConfigs.MovingTypeConfigs;

        platformCausesOfDestroy = PlatformGeneratorConfigs.PlatformConfigs.CauseOfDestroy;

        if (createdPlatform.TryGetComponent(out VerticalMotion verticalMotion))
        {
            VerticalMotionConfig verticalMotionConfig =
                (VerticalMotionConfig)movingTypeConfigs.ToList().Find(x => x is VerticalMotionConfig);

            VerticalMotionConfig.MotionConfigs config = verticalMotionConfig.Value != VerticalMotionConfig.MotionConfigs.Random
                ? verticalMotionConfig.Value
                : verticalMotionConfig.GetConcreteRandomEnumValue();

            if (platformCausesOfDestroy == PlatformCauseOfDestroy.CauseOfDestroy.LateInitialization)
            {
                platformCausesOfDestroy = new PlatformConfigsData().GetPlatformCauseOfDestroyByVerticalMotionConfig(config);

                if (platformCausesOfDestroy == PlatformCauseOfDestroy.CauseOfDestroy.LateInitialization)
                {
                    Debug.LogError("VerticalMotionConfigs shouldn't be Random!");
                }
            }

            verticalMotion.SetMotionConfigs(config);
        }

        if (createdPlatform.TryGetComponent(out CircularMotion circularMotion))
        {
            CircularMotionConfig circularMotionConfig =
                (CircularMotionConfig)movingTypeConfigs.ToList().Find(x => x is CircularMotionConfig);

            CircularMotionConfig.MotionConfigs config = circularMotionConfig.Value != CircularMotionConfig.MotionConfigs.Random
                ? circularMotionConfig.Value
                : circularMotionConfig.GetConcreteRandomEnumValue();

            circularMotion.SetMotionConfigs(config);
        }
    }
}
