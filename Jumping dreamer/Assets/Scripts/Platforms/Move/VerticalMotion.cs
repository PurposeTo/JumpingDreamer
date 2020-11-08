using System;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

/// <summary>
/// Платформа при создании движется от центра
/// </summary>
public class VerticalMotion : PlatformMovable, IMovable, IPooledObject
{
    public bool IsInitialized { get; private set; }

    [SerializeField]
    private bool UpdateMoveDirectionEveryFrame = false;

    private readonly float minVelocityMultiplier = 2f;
    private readonly float maxVelocityMultiplier = 5f;

    private int direction = 0;


    void IPooledObject.OnObjectSpawn()
    {
        SetMotionConfigs();
        IsInitialized = true;
    }


    private void OnDestroy()
    {
        IsInitialized = false;
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


    public PlatformConfigsData.PlatformVerticalCauseOfDestroy GetVerticalCauseOfDestroy()
    {
        switch (direction)
        {
            case 1:
                return PlatformConfigsData.PlatformVerticalCauseOfDestroy.TopBorder;
            case -1:
                return PlatformConfigsData.PlatformVerticalCauseOfDestroy.BottomBorder;
            default:
                throw new Exception($"{direction} is unknown direction!");
        }
    }


    private void SetMotionConfigs()
    {
        PlatformConfigs platformConfigs = WorldGenerationRulesController.Instance.PlatformGeneratorPresenter.PlatformGeneratorConfigs.PlatformConfigs;
        VerticalMotionConfig verticalMotionConfig = (VerticalMotionConfig)platformConfigs.PlatformMovingTypeConfigs
                .ToList()
                .Find(platformMotionConfig => platformMotionConfig is VerticalMotionConfig);

        if (verticalMotionConfig == null) throw new NullReferenceException("verticalMotionConfig can't be null! Check PlatformConfigs!");

        switch (verticalMotionConfig.MotionConfig)
        {
            case VerticalMotionConfig.VerticalMotionConfigs.Up:
                direction = 1;
                break;
            case VerticalMotionConfig.VerticalMotionConfigs.Down:
                direction = -1;
                break;
            case VerticalMotionConfig.VerticalMotionConfigs.Random:
                direction = directionsToChoice[Random.Range(0, directionsToChoice.Length)];
                break;
            default:
                throw new Exception($"{verticalMotionConfig.MotionConfig:D} is unknown motionConfig!");
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
