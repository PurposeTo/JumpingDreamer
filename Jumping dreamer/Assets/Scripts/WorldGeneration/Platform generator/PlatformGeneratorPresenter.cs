using UnityEngine;

[RequireComponent(typeof(PlatformGenerator))]
public class PlatformGeneratorPresenter : MonoBehaviour
{
    [SerializeField] private PlatformGeneratorData platformGeneratorData = null;
    private PlatformGenerator platformGenerator;
    public PlatformGeneratorConfigs PlatformGeneratorConfigs => platformGenerator.PlatformGeneratorConfigs;


    private void Awake()
    {
        platformGenerator = gameObject.GetComponent<PlatformGenerator>();
        platformGenerator.Constructor(platformGeneratorData);
        SetDefaultPlatformGenerationConfigs();
    }


    public void SetNewPlatformGenerationConfigs()
    {
        platformGenerator.SetNewPlatformGenerationConfigs();
    }


    private void SetDefaultPlatformGenerationConfigs()
    {
        platformGenerator.SetDefaultPlatformGenerationConfigs();
    }
}
