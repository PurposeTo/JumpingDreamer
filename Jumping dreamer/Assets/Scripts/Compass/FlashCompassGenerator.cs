using UnityEngine;

public class FlashCompassGenerator : SingletonMonoBehaviour<FlashCompassGenerator>
{
    [SerializeField] private GameObject flashCompass = null;
    //[SerializeField] private Canvas compassCanvas = null;


    public void GenerateFlashCompass(Flash flash)
    {
        //GameObject flashCompassObject = ObjectPooler.Instance.SpawnFromPool(
        //    flashCompass,
        //    compassCanvas.GetComponentInChildren<FlashCompass>().GetComponent<RectTransform>().position,
        //    Quaternion.identity);
        //flashCompassObject.GetComponent<FlashCompass>().Constructor(flash);

        flashCompass.SetActive(true);
        flashCompass.GetComponent<FlashCompass>().Constructor(flash);
    }


    public void TurnOffCompass() => flashCompass.SetActive(false);
}

