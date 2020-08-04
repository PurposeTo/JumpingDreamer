using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Pursuer : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private Vector2 moveDirection;
    private readonly float startVelocityMultiplier = 8f;
    private readonly float finishVelocityMultiplier = 16f;
    private float currentVelocityMultiplier;

    private readonly float startRotationVelocity = 120f;
    private readonly float finishRotationVelocity = 80f;
    private float currentRotationVelocity;

    private readonly float maxLifeTime = 60f;
    private float lifeTimeCounter = 0f;
    private float percentLifeTimeCounter = 0f;


    private float percentageOfTimeSpentByThePlayerMoving;
    private PlayerTactics playerTactics;

    private GameObject target;


    private void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        target = GameManager.Instance.Player;
        playerTactics = GameManager.Instance.PlayerPresenter.PlayerTactics;

        moveDirection = ((Vector2)target.transform.position - rb2d.position).normalized;
    }

    private void Update()
    {
        percentageOfTimeSpentByThePlayerMoving = playerTactics.GetPercentageOfTimeSpentByThePlayerMoving();



        lifeTimeCounter += Time.deltaTime;
        percentLifeTimeCounter = lifeTimeCounter / maxLifeTime;

        currentVelocityMultiplier = Mathf.Lerp(startVelocityMultiplier, finishVelocityMultiplier, percentLifeTimeCounter);
        currentRotationVelocity = Mathf.Lerp(startRotationVelocity, finishRotationVelocity, percentLifeTimeCounter);
    }


    private void FixedUpdate()
    {
        Vector3 toTargetDirection = (target.transform.position - (Vector3)rb2d.position).normalized;

        // Вычисляем кватернион нужного поворота. Вектор forward говорит вокруг какой оси поворачиваться
        Quaternion toTargetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: toTargetDirection);

        Quaternion currentRotation = Quaternion.RotateTowards(transform.rotation, toTargetRotation, currentRotationVelocity * Time.deltaTime);

        rb2d.MoveRotation(currentRotation);
        rb2d.velocity = currentRotation * moveDirection * currentVelocityMultiplier;
    }
}
