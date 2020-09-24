using UnityEngine;

public class Springboard : MonoBehaviour
{
    private bool onlyUpToss; // Костыль, но и фиг с ним. Подбрасывать вверх только если это платформа с PlatformEffector2D.

    private void Awake()
    {
        onlyUpToss = gameObject.TryGetComponent(out PlatformEffector2D _);
    }


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
        playerMovement.TossUp(onlyUpToss);
    }
}
