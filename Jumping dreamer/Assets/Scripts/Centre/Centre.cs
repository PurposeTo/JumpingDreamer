using UnityEngine;

public class Centre : MonoBehaviour
{
    [SerializeField]
    private KillingZone killingZone = null;

    public static readonly float CentreRadius = 9f;

    private PlayerPresenter PlayerPresenter => GameObjectsHolder.Instance.PlayerPresenter;
    

    private void Start()
    {
        PlayerPresenter.PlayerHealth.OnPlayerIsInvulnerable += DeactivateKillingZone;
        SetScale();
    }


    private void OnDestroy()
    {
        PlayerPresenter.PlayerHealth.OnPlayerIsInvulnerable -= DeactivateKillingZone;
    }


    #region методы взаимодействия с игровым объектом "Центр"
    public Vector2 GetToCentreVector(Vector2 position)
    {
        return (Vector2)gameObject.transform.position - position;
    }


    public Vector2 GetToCentreDirection(Vector2 position)
    {
        return GetToCentreVector(position).normalized;
    }


    public float GetToCentreMagnitude(Vector2 position)
    {
        return GetToCentreVector(position).magnitude;
    }


    public Vector2 GetFromCentreVector(Vector2 position)
    {
        return GetToCentreVector(position) * -1f;
    }


    public Vector2 GetFromCentreDirection(Vector2 position)
    {
        return GetToCentreDirection(position) * -1f;
    }


    public float GetFromCentreMagnitude(Vector2 position)
    {
        return GetToCentreMagnitude(position) * -1f;
    }
    #endregion


    private void DeactivateKillingZone(bool deactivate)
    {
        // При выключении KillingZone необходимо поверхность сделать осязаемой
        killingZone.SetIsTriggerZone(!deactivate);
    }


    private void SetScale()
    {
        float Size = CentreRadius * 2f;

        if (!Mathf.Approximately(transform.localScale.x, Size))
        {
            Debug.LogWarning($"Внимание! Размер {gameObject.name} устанавливается из скрипта!");
        }

        transform.localScale = new Vector3(Size, Size, Size);
    }
}
