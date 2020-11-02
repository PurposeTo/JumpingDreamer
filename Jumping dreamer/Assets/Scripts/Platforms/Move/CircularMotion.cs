using System.Linq;
using UnityEngine;

public class CircularMotion : PlatformMovable, IMovable, IPooledObject
{
    private readonly float minVelocityMultiplier = 2.5f;
    private readonly float maxVelocityMultiplier = 10f;

    private int direction;


    private void FixedUpdate()
    {
        MoveAround();
    }


    public void SetMotionConfigs()
    {
        PlatformConfigs platformConfigs = WorldGenerationRulesController.Instance.PlatformGeneratorPresenter.PlatformGeneratorConfigs.PlatformConfigs;
        CircularMotionConfig circularMotionConfig = (CircularMotionConfig)platformConfigs.PlatformMovingTypeConfigs
                .ToList()
                .Find(platformMotionConfig => platformMotionConfig is CircularMotionConfig);

        if (circularMotionConfig == null) throw new System.NullReferenceException("platformMotionConfig can't be null!");

        switch (circularMotionConfig.MotionConfig)
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
                throw new System.Exception($"{circularMotionConfig.MotionConfig:D} is unknown motionConfig!");
        }

        velocityMultiplier = Random.Range(minVelocityMultiplier, maxVelocityMultiplier);
    }


    private void MoveAround()
    {
        Vector2 toCentreDirection = GameObjectsHolder.Instance.Centre.GetToCentreDirection(transform.position);
        moveDirection = GameLogic.GetOrthoNormalizedVector2(toCentreDirection) * direction;
        SetVelocity(moveDirection * velocityMultiplier);
    }


    void IPooledObject.OnObjectSpawn()
    {
        SetMotionConfigs();
    }
}
