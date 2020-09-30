using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformGenerator
{
    public PlatformGenerator(PlatformGeneratorData platformGeneratorData)
    {
        this.platformGeneratorData = platformGeneratorData;
        PlatformGeneratorConfigs = new PlatformGeneratorConfigs(Creating.Default);
    }

    public PlatformGeneratorConfigs PlatformGeneratorConfigs { get; private set; }
    private readonly PlatformGeneratorData platformGeneratorData;

    private float counter = 0f;
    private protected List<Vector2> directionsAroundCircle = new List<Vector2>();


    public void SetNewPlatformGeneratorConfigs()
    {
        this.PlatformGeneratorConfigs = new PlatformGeneratorConfigs(Creating.Random);
    }


    public void Generating()
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
        Vector3 position = GameManager.Instance.CentreObject.transform.position + (Vector3)directionsAroundCircle[0];
        directionsAroundCircle.RemoveAt(0);
        position *= platformGeneratorData.GetCreatingRange(platformConfigs.PlatformCreatingPlace);

        GameObject platformToCreate = platformGeneratorData.GetPlatform(platformConfigs.PlatformMovingTypes);
        GameObject createdPlatform = ObjectPooler.Instance.SpawnFromPool(platformToCreate, position, Quaternion.identity);
    }
}
