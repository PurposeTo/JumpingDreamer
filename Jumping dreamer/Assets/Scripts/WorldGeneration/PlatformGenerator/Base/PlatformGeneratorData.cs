using UnityEngine;

public class PlatformGeneratorData : MonoBehaviour
{
    public GameObject DefaultPlatform;
    public GameObject CircularMotionPlatform;
    public GameObject SpiralMotionPlatformPlatform;

    public PlatformGeneratorState DefaultGeneratorState { get; private set; }
    public PlatformGeneratorState SpiralMotionGeneratorState { get; private set; }


    private void Awake()
    {
        DefaultGeneratorState = new DefaultPlatformGenerator(CircularMotionPlatform);
    }
}
