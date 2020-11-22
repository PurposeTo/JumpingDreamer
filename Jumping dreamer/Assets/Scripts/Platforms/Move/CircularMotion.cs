using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class CircularMotion : PlatformMovable, IMovable
{
    private readonly float minVelocityMultiplier = 2.5f;
    private readonly float maxVelocityMultiplier = 10f;

    private int direction;


    private void FixedUpdate()
    {
        MoveAround();
    }


    public void SetMotionConfigs(CircularMotionConfig.MotionConfigs circularMotionConfigs)
    {
        switch (circularMotionConfigs)
        {
            case CircularMotionConfig.MotionConfigs.Left:
                direction = 1;
                break;
            case CircularMotionConfig.MotionConfigs.Right:
                direction = -1;
                break;
            default:
                throw new Exception($"{circularMotionConfigs} is unknown motionConfig!");
        }

        velocityMultiplier = Random.Range(minVelocityMultiplier, maxVelocityMultiplier);
    }


    private void MoveAround()
    {
        Vector2 toCentreDirection = GameObjectsHolder.Instance.Centre.GetToCentreDirection(transform.position);
        moveDirection = GameLogic.GetOrthoNormalizedVector2(toCentreDirection) * direction;
        SetVelocity(moveDirection * velocityMultiplier);
    }
}
