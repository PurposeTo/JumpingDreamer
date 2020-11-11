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
        SetMotionConfigs(WorldGenerationRulesController.Instance.PlatformGeneratorPresenter.PlatformGeneratorConfigs.PlatformConfigs);
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


    public PlatformCauseOfDestroyConfigsByHight GetPlatformCauseOfDestroyByHight()
    {
        switch (direction)
        {
            case 1:
                return 
                    new PlatformCauseOfDestroyConfigsByHight(PlatformCauseOfDestroyConfigsByHight.PlatformCausesOfDestroyByHight.TopBorder);
            case -1:
                return 
                    new PlatformCauseOfDestroyConfigsByHight(PlatformCauseOfDestroyConfigsByHight.PlatformCausesOfDestroyByHight.BottomBorder);
            default:
                throw new Exception($"{direction} is unknown direction!");
        }
    }


    private void SetMotionConfigs(PlatformConfigs platformConfigs)
    {
        if (!(platformConfigs.MovingTypeConfigs
            .ToList()
            .Find(platformMotionConfig => platformMotionConfig.ParentTier.Value == PlatformMovingTypes.VerticalMotion)
            is VerticalMotionConfig verticalMotionConfig))
        {
            throw new NullReferenceException($"{nameof(verticalMotionConfig)} can't be null! Check PlatformConfigs!");
        }


        switch (verticalMotionConfig.Value)
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
                throw new Exception($"{verticalMotionConfig.Value:D} is unknown motionConfig!");
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
