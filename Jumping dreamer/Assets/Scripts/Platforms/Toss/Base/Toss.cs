using UnityEngine;

public abstract class Toss : MonoBehaviour
{
    private protected enum TossDirectionEnum
    {
        Up,
        Down
    }

    private protected abstract TossDirectionEnum TossDirection { get; }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerPresenter playerPresenter))
        {
            TossPlayer(playerPresenter);
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerPresenter playerPresenter))
        {
            if (collision.enabled)
            {
                TossPlayer(playerPresenter);
            }
        }
    }


    private void TossPlayer(PlayerPresenter playerPresenter)
    {
        if (TossDirection == TossDirectionEnum.Down && playerPresenter.PlayerHealth.IsInvulnerable) return;

        playerPresenter.PlayerMovement.Toss(GetDirecrion(TossDirection));
    }


    private float GetDirecrion(TossDirectionEnum tossDirection)
    {
        switch (tossDirection)
        {
            case TossDirectionEnum.Up:
                return 1f;
            case TossDirectionEnum.Down:
                return -1f;
            default:
                return 0f;
        }
    }
}
