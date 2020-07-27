using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Pursuer : MonoBehaviour
{
    private Rigidbody2D rb2d;

    private Vector2 moveDirection;
    private float velocityMultiplier = 10f;
    private float rotationVelocity = 120f;

    private GameObject target;

    private void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        target = GameManager.Instance.Player;

        moveDirection = ((Vector2)target.transform.position - rb2d.position).normalized;
    }

    private void FixedUpdate()
    {
        Vector3 toTargetDirection = (target.transform.position - (Vector3)rb2d.position).normalized;

        // Вычисляем кватернион нужного поворота. Вектор forward говорит вокруг какой оси поворачиваться
        Quaternion toTargetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: toTargetDirection);

        Quaternion currentRotation = Quaternion.RotateTowards(transform.rotation, toTargetRotation, rotationVelocity * Time.deltaTime);

        rb2d.rotation = currentRotation.eulerAngles.z;
        rb2d.velocity = currentRotation * moveDirection * velocityMultiplier;
    }
}
