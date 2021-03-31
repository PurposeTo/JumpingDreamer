using UnityEngine;
using System.Collections;
using Desdiene.ObjectPoolerAsset;

public class FlashGenerator : MonoBehaviour
{
    private RectTransform flashCompassCanvas;
    private FlashGeneratorData flashGeneratorData;
    private FlashGeneratorConfig flashGeneratorConfig;


    private void Start()
    {
        StartCoroutine(LifeCycleEnumerator());
    }


    public FlashGenerator Constructor(FlashGeneratorData flashGeneratorData, RectTransform flashCompassCanvas)
    {
        if (flashGeneratorData == null) throw new System.ArgumentNullException("flashGeneratorData");
        if (flashCompassCanvas == null) throw new System.ArgumentNullException("flashCompassCanvas");

        this.flashGeneratorData = flashGeneratorData;
        this.flashCompassCanvas = flashCompassCanvas;
        return this;
    }


    public void SetDefaultFlashGenerationConfigs()
    {
        flashGeneratorConfig = FlashGeneratorConfig.GetDefault();
    }


    public void SetRandomFlashGenerationConfigs()
    {
        flashGeneratorConfig = FlashGeneratorConfig.GetRandom();
    }


    private IEnumerator LifeCycleEnumerator()
    {
        while (true)
        {
            yield return new WaitUntil(() => flashGeneratorConfig.IsGenerating);
            GenerateFlash();
            yield return new WaitForSeconds(flashGeneratorConfig.TimePeriodForGeneratingFlashs);
        }
    }


    private void GenerateFlash()
    {
        Flash createdFlash = flashGeneratorData.Flash.SpawnFromPool<Flash>();
        FlashCompass compass = flashGeneratorData.FlashCompass.SpawnFromPool<FlashCompass>().Constructor(createdFlash);
        compass.transform.SetParent(flashCompassCanvas.transform);
    }
}
