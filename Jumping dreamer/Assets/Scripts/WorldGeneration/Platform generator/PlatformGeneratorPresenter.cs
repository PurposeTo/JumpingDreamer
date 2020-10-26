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
    }


    public void SetDefaultPlatformGeneratorConfigs()
    {
        platformGenerator.SetDefaultPlatformGeneratorConfigs();
    }


    public void SetNewPlatformGeneratorConfigs()
    {
        platformGenerator.SetNewPlatformGeneratorConfigs();
    }
}
