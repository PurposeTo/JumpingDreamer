﻿using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private Animator animator;

    private bool isInvulnerable = false;

    private readonly float raiseHeight = 65f;


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
        animator.SetBool("isInvulnerable", true);
        isInvulnerable = true;
    }


    private void SetInvulnerableFalse()
    {
        animator.SetBool("isInvulnerable", false);
        isInvulnerable = false;
    }


    public void RaiseTheDead()
    {
        SetInvulnerableTrue();

        Vector2 toCentreDirection = ((Vector2)GameManager.Instance.Centre.transform.position - rb2D.position).normalized;

        rb2D.velocity = Vector2.zero;


        float impulseVelocity = Mathf.Sqrt(raiseHeight * rb2D.mass * 2 * Gravity.GravityAcceleration * Gravity.GravityScale);
        rb2D.AddForce(-1 * toCentreDirection * impulseVelocity, ForceMode2D.Impulse);

        float fallingTime = Mathf.Sqrt(2 * raiseHeight / (Gravity.GravityAcceleration * Gravity.GravityScale));
        Debug.Log($"Your fallingTime after raise is {fallingTime}");

    }


    private void Die()
    {
        GameMenu.Instance.GameOver();
    }
}
