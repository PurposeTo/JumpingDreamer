using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CircularMotion : PlatformMovable, IMovable, IPooledObject
{
    private readonly float minVelocityMultiplier = 2.5f;
    private readonly float maxVelocityMultiplier = 10f;

    private int direction;


    void IPooledObject.OnObjectSpawn()
    {
        SetMotionConfigs(WorldGenerationRulesController.Instance.PlatformGeneratorPresenter.PlatformGeneratorConfigs.PlatformConfigs);
    }


    private void FixedUpdate()
    {
        MoveAround();
    }


    public void SetMotionConfigs(PlatformConfigs platformConfigs)
    {
        if (!(platformConfigs.MovingTypeConfigs
            .ToList()
            .Find(platformMotionConfig => platformMotionConfig.ParentTier.Value == PlatformMovingTypes.CircularMotion)
            is CircularMotionConfig circularMotionConfig))
        {
            throw new NullReferenceException($"{nameof(circularMotionConfig)} can't be null! Check PlatformConfigs!");
        }

        switch (circularMotionConfig.Value)
        {
            case CircularMotionConfig.CircularMotionConfigs.Left:
                direction = 1;
                break;
            case CircularMotionConfig.CircularMotionConfigs.Right:
                direction = -1;
                break;
            case CircularMotionConfig.CircularMotionConfigs.Random:
                direction = directionsToChoice[Random.Range(0, directionsToChoice.Length)];
                break;
            default:
                throw new Exception($"{circularMotionConfig.Value:D} is unknown motionConfig!");
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
