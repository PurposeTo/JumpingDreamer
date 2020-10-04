using UnityEngine;
using UnityEngine.UI;

public class FlashCompass : MonoBehaviour
{
    [SerializeField] private Canvas compassCanvas = null;
    private RectTransform transformOfCompassCanvas;
    private RectTransform compassTransform;

    private Flash flash;

    private readonly float lowerBoundOfPlayerViewingRange = -135f;
    private readonly float upperBoundOfPlayerViewingRange = 135f;

    private Vector2 defaultPosition => new Vector2(compassOxOffset, compassOyOffset);
    private readonly float defaultTransparency = 0.25f;

    private float compassOxOffset;
    private float compassOyOffset;

    private Vector2 playerDirection => GameManager.Instance.GetToCentreDirection(GameManager.Instance.Player.transform.position) * -1f;

    //private float transparencyWhileBlinking = 1f;
    //private readonly float blinkingRate = 30f;

    private Image image;


    private void Start()
    {
        image = gameObject.GetComponent<Image>();
        transformOfCompassCanvas = compassCanvas.GetComponent<RectTransform>();
        compassTransform = gameObject.GetComponent<RectTransform>();

        compassOxOffset = compassTransform.rect.width / 2;
        compassOyOffset = compassTransform.rect.height / 2;
    }


    private void Update()
    {
        SetCompassCharacteristics();
    }


    public void Constructor(Flash flash)
    {
        this.flash = flash;
    }


    private void SetCompassCharacteristics()
    {
        float differenceAngle = Vector2.SignedAngle(playerDirection, flash.Direction);

        if ((lowerBoundOfPlayerViewingRange <= differenceAngle) && (differenceAngle <= upperBoundOfPlayerViewingRange))
        {
            float differenceAngleMappingOnPlayerViewingRange = (differenceAngle + upperBoundOfPlayerViewingRange) / (upperBoundOfPlayerViewingRange + Mathf.Abs(lowerBoundOfPlayerViewingRange));

            SetCompassPosition(differenceAngleMappingOnPlayerViewingRange);
            SetCompassTransparency(differenceAngleMappingOnPlayerViewingRange);
        }
        else SetDefaultCompassCharacteristics();
    }


    private void SetDefaultCompassCharacteristics()
    {
        compassTransform.position = defaultPosition; // По умолчанию компас находится в левом нижнем углу экрана
        image.color = new Color(image.color.r, image.color.g, image.color.b, defaultTransparency);
    }


    private void SetCompassPosition(float differenceAngleMappingOnPlayerViewingRange)
    {
        // С эффектом "заплытия" за экран
        compassTransform.position = new Vector2(
            Mathf.Lerp(-compassOxOffset, transformOfCompassCanvas.rect.width + compassOxOffset, differenceAngleMappingOnPlayerViewingRange), compassOyOffset);
    }


    private void SetCompassTransparency(float differenceAngleMappingOnPlayerViewingRange)
    {
        float alphaColor;

        // TODO: Сделать нормально
        if (differenceAngleMappingOnPlayerViewingRange <= 0.5f)
        {
            alphaColor = Mathf.Lerp(0.25f, 1f + 0.75f, differenceAngleMappingOnPlayerViewingRange);
        }
        else alphaColor = Mathf.Lerp(1f + 0.75f, 0.25f, differenceAngleMappingOnPlayerViewingRange);

        image.color = new Color(image.color.r, image.color.g, image.color.b, alphaColor);
    }
}
