using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private HashSet<Rigidbody2D> affectedBodies = new HashSet<Rigidbody2D>();
    private const float gravityAcceleration = 9.81f;

    private readonly float gravityScale = 2.5f;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null)
        {
            affectedBodies.Add(collision.attachedRigidbody);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody != null)
        {
            affectedBodies.Remove(collision.attachedRigidbody);
        }
    }


    private void FixedUpdate()
    {
        foreach (Rigidbody2D body in affectedBodies)
        {
            Vector2 dictance = ((Vector2)transform.position - body.position);
            Vector2 forceDirection = dictance.normalized;

            body.AddForce(gravityScale * forceDirection * body.mass * gravityAcceleration);
        }
    }
}
