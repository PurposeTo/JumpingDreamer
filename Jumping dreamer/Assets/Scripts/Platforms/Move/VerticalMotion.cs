using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Платформа при создании движется от центра
/// </summary>
public class VerticalMotion : PlatformMovable, IMovable, IPooledObject
{
    [SerializeField]
    private bool UpdateMoveDirectionEveryFrame = false;

    private readonly float minVelocityMultiplier = 2f;
    private readonly float maxVelocityMultiplier = 5f;

    private int direction = 0;


    void IPooledObject.OnObjectSpawn()
    {

    }


    private void OnDisable()
    {
        direction = 0;
    }

    private void FixedUpdate()
    {
        if (UpdateMoveDirectionEveryFrame)
        {
            UpdateMoveDirection();
            SetVelocity(moveDirection * velocityMultiplier);
        }
    }


    public void SetMotionConfigs(VerticalMotionConfig.VerticalMotionConfigs verticalMotionConfigs)
    {
        switch (verticalMotionConfigs)
        {
            case VerticalMotionConfig.VerticalMotionConfigs.Up:
                direction = 1;
                break;
            case VerticalMotionConfig.VerticalMotionConfigs.Down:
                direction = -1;
                break;
            default:
                throw new Exception($"{verticalMotionConfigs} is unknown motionConfig!");
        }

        velocityMultiplier = Random.Range(minVelocityMultiplier, maxVelocityMultiplier);
        UpdateMoveDirection();
    }


    private void UpdateMoveDirection()
    {
        moveDirection = GetUpdatedMoveDirection() * direction;
        SetVelocity(moveDirection * velocityMultiplier);
    }


    private Vector2 GetUpdatedMoveDirection()
    {
        Vector2 toCentreDirection = GameObjectsHolder.Instance.Centre.GetToCentreDirection(transform.position);
        return toCentreDirection * -1;
    }
}
