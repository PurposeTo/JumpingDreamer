using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out KillingZone _))
        {
            Die();
        }
    }


    private void Die()
    {
        GameMenu.Instance.GameOver();
    }
}
