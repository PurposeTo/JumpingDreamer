using UnityEngine;

public class CircularMotion : MovingPlatform, IMovable
{
    private protected override void Start()
    {
        base.Start();
        velocityMultiplier = 10f;
    }


    private void FixedUpdate()
    {
        MoveAround();
    }


    private void MoveAround()
    {
        Vector2 toCentreDirection = ((Vector2)centre.transform.position - rb2D.position).normalized;
        moveDirection = GameLogic.GetOrthoNormalizedVector2(toCentreDirection);

        SetVelocity(moveDirection * velocityMultiplier);
    }
}
