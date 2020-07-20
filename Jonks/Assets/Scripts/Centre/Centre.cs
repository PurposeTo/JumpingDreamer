using UnityEngine;

public class Centre : MonoBehaviour
{
    [SerializeField]
    private KillingZone killingZone = null;


    private void Start()
    {
        float Size = GameManager.Instance.CentreRadius * 2f;

        if (!Mathf.Approximately(transform.localScale.x, Size))
        {
            Debug.LogWarning($"Внимание! Размер {gameObject.name} устанавливается из скрипта!");
        }

        transform.localScale = new Vector3(Size, Size, Size);
    }


    public void SetIsTriggerKillingZone(bool isTrigger)
    {
        killingZone.SetIsTriggerZone(isTrigger);
    }
}
