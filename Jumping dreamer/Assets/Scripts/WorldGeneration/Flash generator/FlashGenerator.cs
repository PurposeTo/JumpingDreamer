using UnityEngine;
using System.Collections;

public class FlashGenerator : MonoBehaviour
{
    private RectTransform flashCompassCanvas;
    private FlashGeneratorData flashGeneratorData;
    private FlashGeneratorConfig flashGeneratorConfig;


    private void Start()
    {
        StartCoroutine(LifeCycleEnumerator());
    }


    public void Constructor(FlashGeneratorData flashGeneratorData, RectTransform flashCompassCanvas)
    {
        if (flashGeneratorData == null) throw new System.ArgumentNullException("flashGeneratorData");
        if (flashCompassCanvas == null) throw new System.ArgumentNullException("flashCompassCanvas");

        this.flashGeneratorData = flashGeneratorData;
        this.flashCompassCanvas = flashCompassCanvas;
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
        GameObject createdFlash = ObjectPooler.Instance.SpawnFromPool(flashGeneratorData.Flash, Vector2.zero, Quaternion.identity);
        GameObject compass = ObjectPooler.Instance.SpawnFromPool(flashGeneratorData.FlashCompass, Vector2.zero, Quaternion.identity, flashCompassCanvas.transform);
        compass.GetComponent<FlashCompass>().Constructor(createdFlash.GetComponent<Flash>());
    }
}
