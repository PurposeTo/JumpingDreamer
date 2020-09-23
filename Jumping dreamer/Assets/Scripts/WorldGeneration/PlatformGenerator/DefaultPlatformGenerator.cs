using UnityEngine;

public class DefaultPlatformGenerator : PlatformGeneratorState
{
    public DefaultPlatformGenerator(GameObject defaultPlatform)
    {
        platform = defaultPlatform;
    }


    private protected override float Delay => 0.4f;
    private readonly GameObject platform;


    private protected override void GeneratePlatform()
    {
        // Позиция равна первому элементу в списке. После использования позиции, убрать из списка
        Vector3 position = GameManager.Instance.CentreObject.transform.position + (Vector3)directionsAroundCircle[0];
        directionsAroundCircle.RemoveAt(0);

        ObjectPooler.Instance.SpawnFromPool(platform, position, Quaternion.identity);
    }
}
