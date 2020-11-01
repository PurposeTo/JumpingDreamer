using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator animator;

    private bool isInvulnerable = false;

    private readonly float maxRaiseHeight = 65f;
    private readonly float minRaiseHeight = 20f;


    public event Action<bool> OnPlayerIsInvulnerable;
    public event Action<bool> OnPlayerDie;


    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isInvulnerable && collision.gameObject.TryGetComponent(out KillingZone _))
        {
            Die();
        }
    }


    // Спасибо юнити аниматору, что он не видит методы, в которые необходимо передавать аргументы
    private void SetInvulnerableTrue()
    {
        SetInvulnerable(true);
    }


    private void SetInvulnerableFalse()
    {
        SetInvulnerable(false);
    }


    private void SetInvulnerable(bool isInvulnerable)
    {
        animator.SetBool("isInvulnerable", isInvulnerable);
        this.isInvulnerable = isInvulnerable;
        OnPlayerIsInvulnerable?.Invoke(isInvulnerable);
    }


    public void RaiseTheDead()
    {
        OnPlayerDie?.Invoke(false);
        SetInvulnerableTrue();

        Vector2 toCentreVector = GameManager.Instance.GetToCentreVector(rb2D.position);
        Vector2 toCentreDirection = toCentreVector.normalized;
        float toCentreDistance = toCentreVector.magnitude - Centre.CentreRadius;

        rb2D.velocity = Vector2.zero;

        float differenceBetweenMaxRaiseHeightAndCurrentPosition = maxRaiseHeight - toCentreDistance / 5f;
        float raiseHeight = differenceBetweenMaxRaiseHeightAndCurrentPosition >= minRaiseHeight ? differenceBetweenMaxRaiseHeightAndCurrentPosition : minRaiseHeight;

        float impulseVelocity = Mathf.Sqrt(raiseHeight * rb2D.mass * 2 * Gravity.GravityAcceleration * Gravity.GravityScale);
        rb2D.AddForce(-1 * toCentreDirection * impulseVelocity, ForceMode2D.Impulse);

        float fallingTime = Mathf.Sqrt(2 * (raiseHeight + toCentreDistance) / (Gravity.GravityAcceleration * Gravity.GravityScale));
        // fallingTime учитывает только время падения; так же есть время взлёта
        Debug.Log($"Your falling time to the surface of the Centre after raise is {fallingTime}");
    }


    private void Die()
    {
        OnPlayerDie?.Invoke(true);
        GameManager.Instance.GameOver();
    }
}
