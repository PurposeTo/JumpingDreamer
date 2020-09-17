using UnityEngine;

public class CircularMotion : MovingPlatform, IMovable
{
    private int[] directionsToChoice = { -1, 1 };

    private int direction = 1;
    private float speed = 1f;


    private void OnEnable()
    {
        direction = directionsToChoice[Random.Range(0, directionsToChoice.Length)];
        speed = Random.Range(0.25f, 1f);
    }


    private void Start()
    {
        velocityMultiplier = 10f;
    }


    private void FixedUpdate()
    {
        MoveAround();
    }


    private void MoveAround()
    {
        Vector2 toCentreDirection = ((Vector2)centre.transform.position - rb2D.position).normalized;
        moveDirection = GameLogic.GetOrthoNormalizedVector2(toCentreDirection) * speed * direction;

        SetVelocity(moveDirection * velocityMultiplier);
    }
}
