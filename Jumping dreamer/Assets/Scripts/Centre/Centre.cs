using UnityEngine;

public class Centre : MonoBehaviour
{
    [SerializeField]
    private KillingZone killingZone = null;

    public static readonly float CentreRadius = 9f;

    private PlayerPresenter PlayerPresenter => GameManager.Instance.PlayerPresenter;
    

    private void Start()
    {
        PlayerPresenter.PlayerHealth.OnPlayerIsInvulnerable += DeactivateKillingZone;
        SetScale();
    }


    private void OnDestroy()
    {
        PlayerPresenter.PlayerHealth.OnPlayerIsInvulnerable -= DeactivateKillingZone;
    }


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
