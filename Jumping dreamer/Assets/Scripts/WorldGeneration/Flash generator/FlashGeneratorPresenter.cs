using UnityEngine;

[RequireComponent(typeof(FlashGenerator))]
public class FlashGeneratorPresenter : MonoBehaviour
{
    [SerializeField] private RectTransform flashCompassCanvas = null;
    [SerializeField] private FlashGeneratorData flashGeneratorData = null;
    private FlashGenerator flashGenerator;


    private void Awake()
    {
        flashGenerator = gameObject.GetComponent<FlashGenerator>();
        flashGenerator.Constructor(flashGeneratorData, flashCompassCanvas);
    }

}
