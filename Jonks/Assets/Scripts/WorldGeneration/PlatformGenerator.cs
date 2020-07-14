using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlatformGenerator : SingletonMonoBehaviour<PlatformGenerator>
{
    public GameObject DefaultPlatform;

    private float delay = 0.625f;
    private float counter;

    private List<Vector2> directionsAroundCircle = new List<Vector2>();


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

        Vector2[] vector2sDirections = GetVector2sDirectionsAroundCircle(distanceAngle);

        foreach (Vector2 direction in vector2sDirections)
        {
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
            if (directionsAroundCircle.Count == 0)
            {
                // Расстояние между точками - 5 градусов
                Vector2[] vector2sDirectionsArray = GetVector2sDirectionsAroundCircle(5f);
                GameLogic.Shuffle(vector2sDirectionsArray);
                directionsAroundCircle = vector2sDirectionsArray.ToList();
                Debug.Log("Update directionsAroundCircle array in Platform generator");

            }

            // Позиция равна первому элементу в списке. После использования позиции, убрать из списка
            Vector3 position = GameManager.Instance.Centre.transform.position + (Vector3)directionsAroundCircle[0];
            directionsAroundCircle.RemoveAt(0);

            ObjectPooler.Instance.SpawnFromPool(platform, position, Quaternion.identity);
            counter = delay;
        }
    }


    /// <summary>
    /// Генерирует массив векторов, точки которых описывают круг
    /// </summary>
    /// <param name="distanceAngle">Угол, на расстоянии которого будут распологаться точки</param>
    private Vector2[] GetVector2sDirectionsAroundCircle(float distanceAngle)
    {
        List<Vector2> vector2sDirections = new List<Vector2>();

        int dotsCount = (int)(360f / distanceAngle);

        for (int i = 0; i < dotsCount; i++)
        {
            float rotateAngle = distanceAngle * i;
            Quaternion rotation = Quaternion.Euler(0, 0, rotateAngle);
            Vector2 direction = rotation * Vector3.up;

            vector2sDirections.Add(direction);
        }

        return vector2sDirections.ToArray();
    }
}
