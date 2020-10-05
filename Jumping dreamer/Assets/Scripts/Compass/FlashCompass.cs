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

    private readonly float maxTransparency = 1f;
    private float transparencyRange => maxTransparency - defaultTransparency;

    /// <summary>
    /// Для смещения центра компаса относительно угла (к которому привязан компас)
    /// </summary>
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
        if (flash != null) // Добавил эту проверку, так как пока нет локиги спавна компаса
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
            Mathf.Lerp(transformOfCompassCanvas.rect.width + compassOxOffset, -compassOxOffset, differenceAngleMappingOnPlayerViewingRange), compassOyOffset);
    }


    private void SetCompassTransparency(float differenceAngleMappingOnPlayerViewingRange)
    {
        float alphaColor;

        // TODO: Сделать нормально
        // Сейчас, для того , чтобы при положении компаса на середине экрана (0,5f) значении прозрачности было максимальным (1f), диапозон прозрачности ИСКУССТВЕННО увеличивается на значение разницы между максимальным и минимальным значениями прозрачности. 
        if (differenceAngleMappingOnPlayerViewingRange <= 0.5f)
        {
            alphaColor = Mathf.Lerp(defaultTransparency, maxTransparency + transparencyRange, differenceAngleMappingOnPlayerViewingRange);
        }
        else alphaColor = Mathf.Lerp(maxTransparency + transparencyRange, defaultTransparency, differenceAngleMappingOnPlayerViewingRange);

        image.color = new Color(image.color.r, image.color.g, image.color.b, alphaColor);
    }
}
