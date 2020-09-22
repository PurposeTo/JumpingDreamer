using UnityEngine;

public class SpiralMotionPlatformGenerator : PlatformGeneratorState
{
    public SpiralMotionPlatformGenerator(GameObject spiralMotionPlatform)
    {
        platform = spiralMotionPlatform;
    }

    private protected override float Delay => 0.25f;
    private readonly GameObject platform;


    private protected override void GeneratePlatform()
    {
        // Позиция равна первому элементу в списке. После использования позиции, убрать из списка
        Vector3 position = GameManager.Instance.CentreObject.transform.position + (Vector3)directionsAroundCircle[0];
        directionsAroundCircle.RemoveAt(0);

        // Создать объект только если он будет ВНЕ видимости камеры
        ObjectPooler.Instance.SpawnFromPool(platform, position, Quaternion.identity);
    }
}
