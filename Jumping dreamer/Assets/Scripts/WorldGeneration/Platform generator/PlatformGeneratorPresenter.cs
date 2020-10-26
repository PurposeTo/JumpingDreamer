using UnityEngine;

public class PlatformGeneratorPresenter : MonoBehaviour
{
    [SerializeField] private PlatformGeneratorData platformGeneratorData = null;
    private PlatformGenerator platformGenerator;
    public PlatformGeneratorConfigs PlatformGeneratorConfigs => platformGenerator.PlatformGeneratorConfigs;


    private void Awake()
    {
        platformGenerator = new PlatformGenerator(platformGeneratorData);
    }


    private void Update()
    {
        platformGenerator.Generating();
    }


    public void SetDefaultPlatformGeneratorConfigs()
    {
        platformGenerator.SetDefaultPlatformGeneratorConfigs();
    }


    public void StartPlatformGeneratorInitialization()
    {
        platformGenerator.GenerateRingFromVerticalMotionPlatforms();
    }


    public void SetNewPlatformGeneratorConfigs()
    {
        platformGenerator.SetNewPlatformGeneratorConfigs();
    }
}
