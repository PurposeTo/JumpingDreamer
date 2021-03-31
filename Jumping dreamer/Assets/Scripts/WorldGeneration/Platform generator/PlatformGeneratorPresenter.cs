using Desdiene.SuperMonoBehaviourAsset;
using UnityEngine;

[RequireComponent(typeof(PlatformGenerator))]
public class PlatformGeneratorPresenter : SuperMonoBehaviour
{
    [SerializeField] private PlatformGeneratorData platformGeneratorData = null;
    private PlatformGenerator platformGenerator;
    public PlatformGeneratorConfigs PlatformGeneratorConfigs => platformGenerator.PlatformGeneratorConfigs;


    protected override void AwakeWrapped()
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
