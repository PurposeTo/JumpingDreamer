using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class PlatformGeneratorState
{
    private protected abstract float Delay { get; }
    private float counter = 0f;
    private protected List<Vector2> directionsAroundCircle = new List<Vector2>();


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

            counter = Delay;
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


    private protected abstract void GeneratePlatform();
}
