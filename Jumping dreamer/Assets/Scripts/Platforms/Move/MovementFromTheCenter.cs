using UnityEngine;

/// <summary>
/// Платформа при создании движется от центра
/// </summary>
public class MovementFromTheCenter : PlatformMovable, IMovable, IPooledObject
{
    [SerializeField]
    private bool UpdateMoveDirectionEveryFrame = false;


    private void FixedUpdate()
    {
        if (UpdateMoveDirectionEveryFrame)
        {
            UpdateMoveDirection();
            SetVelocity(moveDirection * velocityMultiplier);
        }
    }


    /// <summary>
    /// Инициализирует платформу
    /// </summary>
    private void InitializePlatform() // todo: Метод должен принимать параметр. Платформа движется из Центра; платформа движется наверх; платформа движется вниз
    {
        // Если платформа не в центре
        if (transform.position != GameManager.Instance.CentreObject.transform.position) moveDirection = GetUpdatedMoveDirection();
        // Если платформа в центре
        else moveDirection = Random.insideUnitCircle.normalized;

        velocityMultiplier = Random.Range(2f, 5f);
        SetVelocity(moveDirection * velocityMultiplier);
    }


    private void UpdateMoveDirection()
    {
        if (transform.position != GameManager.Instance.CentreObject.transform.position)
        {
            moveDirection = GetUpdatedMoveDirection();
        }
    }


    private Vector2 GetUpdatedMoveDirection()
    {
        Vector2 toCentreDirection = GameManager.Instance.GetToCentreDirection(transform.position);
        return toCentreDirection * -1;
    }


    void IPooledObject.OnObjectSpawn()
    {
        InitializePlatform();
    }
}
