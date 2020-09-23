using UnityEngine;

public class Springboard : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerPresenter playerPresenter))
        {
            if (collision.enabled)
            {
                TossUp(playerPresenter.PlayerMovement);
            }

        }
    }


    private void TossUp(PlayerMovement playerMovement)
    {
        playerMovement.TossUp();
    }
}
