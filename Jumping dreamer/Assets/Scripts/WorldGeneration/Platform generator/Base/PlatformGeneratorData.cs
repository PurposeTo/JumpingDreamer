using System.Linq;
using UnityEngine;


//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/PlatformGeneratorData")]
public class PlatformGeneratorData : ScriptableObject
{
    public GameObject VerticalMotionPlatform;
    public GameObject CircularMotionPlatform;
    public GameObject SpiralMotionPlatformPlatform;

    public const float PlatformAvailableHighestArea = 80f;

    public GameObject GetPlatform(PlatformConfigsData.PlatformMovingType[] platformMovingTypes)
    {
        if (platformMovingTypes == null || platformMovingTypes.Length == 0) throw new System.Exception("PlatformMovingTypes can't being empty!");

        bool isPlatformVerticalMotion = platformMovingTypes.Contains(PlatformConfigsData.PlatformMovingType.VerticalMotion);
        bool isPlatformCircularMotion = platformMovingTypes.Contains(PlatformConfigsData.PlatformMovingType.CircularMotion);

        if (isPlatformVerticalMotion && isPlatformCircularMotion)
        {
            return SpiralMotionPlatformPlatform;
        }
        else if(isPlatformVerticalMotion && !isPlatformCircularMotion)
        {
            return VerticalMotionPlatform;
        }
        else if (isPlatformCircularMotion && !isPlatformVerticalMotion)
        {
            return CircularMotionPlatform;
        }
        else
        {
            throw new System.Exception($"{platformMovingTypes} is unknown platformMovingTypes!");
        }
    }


    public float GetCreatingRange(PlatformConfigsData.PlatformCreatingPlace platformCreatingPlace)
    {
        switch (platformCreatingPlace)
        {
            case PlatformConfigsData.PlatformCreatingPlace.InRandomArea:
                return Random.Range(Centre.CentreRadius + 1f, PlatformAvailableHighestArea);
            case PlatformConfigsData.PlatformCreatingPlace.InCentre:
                return 1f; // Один, потому что мы умножем на возвращаемое значение вектор создания.
            case PlatformConfigsData.PlatformCreatingPlace.InHighestArea:
                float halfWay = (PlatformAvailableHighestArea - Centre.CentreRadius) / 2f;
                return Random.Range(halfWay, PlatformAvailableHighestArea);
            default:
                throw new System.Exception($"{platformCreatingPlace} is unknown PlatformCreatingPlace!");
        }
    }
}
