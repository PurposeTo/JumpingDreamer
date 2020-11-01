using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Pursuer : MonoBehaviour, IPooledObject
{
    #region enemy data
    private readonly float startVelocityMultiplier = 8f;
    private readonly float finishVelocityMultiplier = 16f;

    private readonly float startRotationVelocity = 140f;
    private readonly float finishRotationVelocity = 80f;

    private readonly float maxLifeTime = 60f;
    #endregion

    private Rigidbody2D rb2d;

    private Vector2 moveDirection;

    private float currentRotationVelocity;

    private float currentVelocityMultiplier;

    private float lifeTimeCounter = 0f;
    private float percentLifeTimeCounter;

    private PlayerTactics playerTactics;

    private GameObject target;


    private void Awake()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out PlayerPresenter _))
        {
            gameObject.SetActive(false);
        }
    }


    private void Update()
    {
        lifeTimeCounter += Time.deltaTime;
        percentLifeTimeCounter = lifeTimeCounter / maxLifeTime;
    }


    private void FixedUpdate()
    {
        GameObject target = ImportantGameObjectsHolder.Instance.PlayerPresenter.gameObject;

        Vector3 toTargetDirection = (target.transform.position - (Vector3)rb2d.position).normalized;

        // Вычисляем кватернион нужного поворота. Вектор forward говорит вокруг какой оси поворачиваться
        Quaternion toTargetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: toTargetDirection);

        Quaternion currentRotation = Quaternion.RotateTowards(transform.rotation, toTargetRotation, currentRotationVelocity * Time.deltaTime);

        moveDirection = currentRotation * Vector3.up; // Повернуть вектор движения

        rb2d.MoveRotation(currentRotation); // Повернуться "лицом" к цели
        rb2d.velocity = moveDirection * currentVelocityMultiplier;

        Debug.DrawRay(transform.position, moveDirection.normalized * 3, Color.green, 2f); // Вектор непонятно куда
        Debug.DrawRay(transform.position, (toTargetRotation * Vector3.up).normalized * 3, Color.yellow, 2f); // Вектор точный
    }


    // Метод для аниматора - выключить объект
    private void DisableObject()
    {
        gameObject.SetActive(false);
    }


    void IPooledObject.OnObjectSpawn()
    {
        playerTactics = ImportantGameObjectsHolder.Instance.PlayerPresenter.PlayerTactics;
        float percentageOfTimeSpentByThePlayerMoving = playerTactics.PercentageOfTimeSpentByThePlayerMoving;
        currentVelocityMultiplier = Mathf.Lerp(startVelocityMultiplier, finishVelocityMultiplier, percentageOfTimeSpentByThePlayerMoving);
        currentRotationVelocity = Mathf.Lerp(startRotationVelocity, finishRotationVelocity, percentageOfTimeSpentByThePlayerMoving);
    }
}
