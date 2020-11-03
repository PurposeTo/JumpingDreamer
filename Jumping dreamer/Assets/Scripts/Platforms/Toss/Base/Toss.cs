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
            TossUp(playerPresenter.PlayerMovement);
        }
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
        playerMovement.TossUp(GetDirecrion(TossDirection));
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
