using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour
{
    private HashSet<Rigidbody2D> affectedBodies = new HashSet<Rigidbody2D>();

    public const float GravityAcceleration = 9.81f;
    public const float GravityScale = 3f;


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

            body.AddForce(GravityScale * forceDirection * body.mass * GravityAcceleration);
        }
    }
}
