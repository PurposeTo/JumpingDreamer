using UnityEngine;
using System.Collections;

public class FlashGenerator : MonoBehaviour
{
    private RectTransform flashCompassCanvas;
    private FlashGeneratorData flashGeneratorData;

    private readonly float startDelay = 20f;
    private readonly float spawnFrequency = 5f;


    private void Start()
    {
        StartCoroutine(LifeCycleEnumerator());
    }


    public void Constructor(FlashGeneratorData flashGeneratorData, RectTransform flashCompassCanvas)
    {
        if(flashGeneratorData == null) throw new System.ArgumentNullException("flashGeneratorData");
        if(flashCompassCanvas == null) throw new System.ArgumentNullException("flashCompassCanvas");

        this.flashGeneratorData = flashGeneratorData;
        this.flashCompassCanvas = flashCompassCanvas;
    }


    private IEnumerator LifeCycleEnumerator()
    {
        yield return new WaitForSeconds(startDelay);

        WaitForSeconds waitForSeconds = new WaitForSeconds(spawnFrequency);
        while (true)
        {
            GenerateFlash();
            yield return waitForSeconds;
        }
    }


    private void GenerateFlash()
    {
        GameObject createdFlash = ObjectPooler.Instance.SpawnFromPool(flashGeneratorData.Flash, Vector2.zero, Quaternion.identity);
        GameObject compass = ObjectPooler.Instance.SpawnFromPool(flashGeneratorData.FlashCompass, Vector2.zero, Quaternion.identity, flashCompassCanvas.transform);
        compass.GetComponent<FlashCompass>().Constructor(createdFlash.GetComponent<Flash>());
    }
}
