using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FlashCompass : MonoBehaviour, IPooledObject
{
    public AnimationCurve ChangingInitialTransparencyCurve;
    public AnimationCurve ChangingTransparencyCurve;

    private RectTransform compassTransform;
    private Image image;

    private Flash flash;

    private float lowerBoundOfPlayerViewingRange => -upperBoundOfPlayerViewingRange;
    private readonly float upperBoundOfPlayerViewingRange = 180f;

    private readonly float initialTransparency = 0f;
    //private readonly float maxTransparency = 1f;

    private Vector2 compassInitialScale;

    /// <summary>
    /// Смещение компаса относительно позиции якоря
    /// </summary>
    private float compassOxOffset;
    private float compassOyOffset;

    private Vector2 playerDirection => GameManager.Instance.GetToCentreDirection(GameManager.Instance.Player.transform.position) * -1f;

    private Coroutine lifeCycleRoutine;
    private Coroutine turnOnCompassAnimationRoutine;
    private Coroutine flashCompassOperationRoutine;
    private Coroutine turnOffCompassAnimationRoutine;


    private void Awake()
    {
        image = gameObject.GetComponent<Image>();
        compassTransform = gameObject.GetComponent<RectTransform>();
    }


    private void Start()
    {
        compassInitialScale = compassTransform.sizeDelta;

        compassOxOffset = compassTransform.rect.width / 2;
        compassOyOffset = compassTransform.rect.height / 2;
    }


    private void OnDisable()
    {
        flash = null;
        RepairFlashCompass();
    }


    //private void Update()
    //{
    //    if (flash != null) SetCompassCharacteristics();
    //}


    public void Constructor(Flash flash)
    {
        if (flash == null)
        {
            Debug.LogError("Не установлена ссылка на вспышку для компаса!");
            gameObject.SetActive(false);
            return;
        }

        this.flash = flash;

        if (lifeCycleRoutine == null) lifeCycleRoutine = StartCoroutine(LifeCycleEnumerator());
    }


    private IEnumerator LifeCycleEnumerator()
    {
        if (turnOnCompassAnimationRoutine == null)
        {
            turnOnCompassAnimationRoutine = StartCoroutine(TurnOnCompassAnimationEnumerator());
            yield return turnOnCompassAnimationRoutine;
        }

        // Для того, чтобы не дергалось, необходимо менять позицию в Update()
        if (flashCompassOperationRoutine == null)
        {
            flashCompassOperationRoutine = StartCoroutine(FlashCompassOperationEnumerator());
            yield return flashCompassOperationRoutine;
        }

        //yield return new WaitUntil(() => flash.IsFlashKillingZoneActive);

        if (turnOffCompassAnimationRoutine == null)
        {
            turnOffCompassAnimationRoutine = StartCoroutine(TurnOffCompassAnimationEnumerator());
            yield return turnOffCompassAnimationRoutine;
        }

        gameObject.SetActive(false);

        lifeCycleRoutine = null;
    }


    private IEnumerator TurnOnCompassAnimationEnumerator()
    {
        float differenceAngleMappingOnPlayerViewingRange = CalculateDifferenceAngleMapping();

        float alphaColor = CalculateTransparency(differenceAngleMappingOnPlayerViewingRange/*, initialTransparency, maxTransparency*/);
        float timer = 0f;

        // Изменение прозрачности в соответствии с графиком изменения начальной прозрачности
        while (image.color.a < alphaColor)
        {
            timer += Time.deltaTime;

            if (ChangingInitialTransparencyCurve.Evaluate(timer) > alphaColor)
            {
                image.color = new Color(image.color.r, image.color.g, image.color.b, alphaColor);
            }
            else image.color = new Color(image.color.r, image.color.g, image.color.b, ChangingInitialTransparencyCurve.Evaluate(timer));

            yield return null;
        }

        turnOnCompassAnimationRoutine = null;
    }


    private IEnumerator FlashCompassOperationEnumerator()
    {
        while (!flash.IsFlashKillingZoneActive)
        {
            SetCompassCharacteristics();
            yield return null;
        }

        flashCompassOperationRoutine = null;
    }


    private IEnumerator TurnOffCompassAnimationEnumerator()
    {
        float timer = 0f;
        float scalingTime = 0.2f;
        float scale = 1.5f;

        float originalWidth = compassTransform.sizeDelta.x;
        float originalHeight = compassTransform.sizeDelta.y;
        float originalOyPosition = compassTransform.anchoredPosition.y;

        while (timer < scalingTime)
        {
            timer += Time.deltaTime;

            if (timer > scalingTime) timer = scalingTime;

            compassTransform.sizeDelta = new Vector2(Mathf.Lerp(originalWidth, originalWidth * scale, timer / scalingTime), Mathf.Lerp(originalHeight, originalHeight * scale, timer / scalingTime));
            compassTransform.anchoredPosition = new Vector2(compassTransform.anchoredPosition.x, Mathf.Lerp(originalOyPosition, originalOyPosition * scale, timer / scalingTime));
            yield return null;
        }

        turnOffCompassAnimationRoutine = null;
    }


    private float CalculateDifferenceAngleMapping()
    {
        float differenceAngle = Vector2.SignedAngle(playerDirection, flash.Direction);
        return (differenceAngle + upperBoundOfPlayerViewingRange) / (upperBoundOfPlayerViewingRange + Mathf.Abs(lowerBoundOfPlayerViewingRange));
    }


    private void SetCompassCharacteristics()
    {
        float differenceAngleMappingOnPlayerViewingRange = CalculateDifferenceAngleMapping();

        SetCompassPosition(differenceAngleMappingOnPlayerViewingRange);
        image.color = new Color(image.color.r,
                                image.color.g,
                                image.color.b,
                                ChangingTransparencyCurve.Evaluate(differenceAngleMappingOnPlayerViewingRange));
    }


    private void SetCompassPosition(float differenceAngleMappingOnPlayerViewingRange)
    {
        compassTransform.anchorMin = new Vector2(1f - differenceAngleMappingOnPlayerViewingRange, 0f);
        compassTransform.anchorMax = new Vector2(1f - differenceAngleMappingOnPlayerViewingRange, 0f);

        // С эффектом "заплытия" за экран
        float compassOxPosition = Mathf.Lerp(compassTransform.anchorMin.x + compassOxOffset, compassTransform.anchorMin.x - compassOxOffset, differenceAngleMappingOnPlayerViewingRange);

        compassTransform.anchoredPosition = new Vector2(compassOxPosition, compassTransform.anchorMin.y + compassOyOffset);
    }


    private float CalculateTransparency(float differenceAngleMappingOnPlayerViewingRange/*, float minTransparency, float maxTransparency*/)
    {
        float alphaColor;

        if (differenceAngleMappingOnPlayerViewingRange > 0.5f)
        {
            alphaColor = Mathf.InverseLerp(0f, 0.5f, 1f - differenceAngleMappingOnPlayerViewingRange);
            //GameLogic.MapValueFromOneToAnotherRange(1f - differenceAngleMappingOnPlayerViewingRange, 0f, 0.5f, minTransparency, maxTransparency);
        }
        else
        {
            //alphaColor = GameLogic.MapValueFromOneToAnotherRange(differenceAngleMappingOnPlayerViewingRange, 0f, 0.5f, minTransparency, maxTransparency);
            alphaColor = Mathf.InverseLerp(0f, 0.5f, differenceAngleMappingOnPlayerViewingRange);
        }

        return alphaColor;
    }


    private void RepairFlashCompass() => compassTransform.sizeDelta = compassInitialScale;


    void IPooledObject.OnObjectSpawn()
    {
        // Нужно? Или начинать изменять с 0 позиции графика?
        image.color = new Color(image.color.r, image.color.g, image.color.b, initialTransparency);
    }
}
