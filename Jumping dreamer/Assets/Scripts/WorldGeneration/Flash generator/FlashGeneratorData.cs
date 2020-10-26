using UnityEngine;

//[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/FlashGeneratorData")]
public class FlashGeneratorData : ScriptableObject
{
    public GameObject Flash => flash;
    [SerializeField] private GameObject flash = null;

    public GameObject FlashCompass => flashCompass;
    [SerializeField] private GameObject flashCompass = null;
}
