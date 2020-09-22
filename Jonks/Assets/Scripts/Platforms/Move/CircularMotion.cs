using UnityEngine;

public class CircularMotion : MovingPlatform, IMovable
{
    private readonly int[] directionsToChoice = { -1, 1 };
    private readonly float minVelocityMultiplier = 2.5f;
    private readonly float maxVelocityMultiplier = 10f;

    private int direction;


    private void OnEnable()
    {
        direction = directionsToChoice[Random.Range(0, directionsToChoice.Length)];
        velocityMultiplier = Random.Range(minVelocityMultiplier, maxVelocityMultiplier);

    }


    private void FixedUpdate()
    {
        MoveAround();
    }


    private void MoveAround()
    {
        Vector2 toCentreDirection = ((Vector2)GameManager.Instance.CentreObject.transform.position - rb2D.position).normalized;
        moveDirection = GameLogic.GetOrthoNormalizedVector2(toCentreDirection) * direction;

        SetVelocity(moveDirection * velocityMultiplier);
    }
}
