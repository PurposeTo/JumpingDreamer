using System.Linq;
using UnityEngine;

/// <summary>
/// Платформа при создании движется от центра
/// </summary>
public class VerticalMotion : PlatformMovable, IMovable, IPooledObject
{
    [SerializeField]
    private bool UpdateMoveDirectionEveryFrame = false;

    private readonly float minVelocityMultiplier = 2f;
    private readonly float maxVelocityMultiplier = 5f;

    public int Direction { get; private set; }

    private void FixedUpdate()
    {
        if (UpdateMoveDirectionEveryFrame)
        {
            UpdateMoveDirection();
            SetVelocity(moveDirection * velocityMultiplier);
        }
    }


    public void SetMotionConfigs()
    {
        PlatformConfigs platformConfigs = WorldGenerationRulesController.Instance.PlatformGeneratorPresenter.PlatformGeneratorConfigs.PlatformConfigs;
        VerticalMotionConfig verticalMotionConfig = (VerticalMotionConfig)platformConfigs.PlatformMovingTypeConfigs
                .ToList()
                .Find(platformMotionConfig => platformMotionConfig is VerticalMotionConfig);

        if (verticalMotionConfig == null) throw new System.NullReferenceException("verticalMotionConfig can't be null!");

        switch (verticalMotionConfig.MotionConfig)
        {
            case VerticalMotionConfig.VerticalMotionConfigs.Up:
                Direction = 1;
                break;
            case VerticalMotionConfig.VerticalMotionConfigs.Down:
                Direction = -1;
                break;
            case VerticalMotionConfig.VerticalMotionConfigs.Random:
                Direction = directionsToChoice[Random.Range(0, directionsToChoice.Length)];
                break;
            default:
                throw new System.Exception($"{verticalMotionConfig.MotionConfig:D} is unknown motionConfig!");
        }

        velocityMultiplier = Random.Range(minVelocityMultiplier, maxVelocityMultiplier);
        UpdateMoveDirection();
    }


    private void UpdateMoveDirection()
    {
        moveDirection = GetUpdatedMoveDirection() * Direction;
        SetVelocity(moveDirection * velocityMultiplier);
    }


    private Vector2 GetUpdatedMoveDirection()
    {
        Vector2 toCentreDirection = GameObjectsHolder.Instance.Centre.GetToCentreDirection(transform.position);
        return toCentreDirection * -1;
    }

    void IPooledObject.OnObjectSpawn()
    {
        SetMotionConfigs();
    }
}
