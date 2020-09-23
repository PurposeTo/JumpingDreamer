using System.Collections;
using UnityEngine;

/// <summary>
/// Платформа при создании движется от центра
/// </summary>
public class MovementFromTheCenter : MovingPlatform, IMovable, IPooledObject
{
    private float lifeDictance; // Максимальное расстояние от центра до точки, где платформы еще могут существовать
    private readonly float minlifeDictance = 50f;
    private readonly float maxlifeDictance = 80f;


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
    private void InitializePlatform()
    {
        // Если платформа не в центре
        if (transform.position != GameManager.Instance.CentreObject.transform.position)
        {
            moveDirection = GetUpdatedMoveDirection();
        }
        else // Если платформа в центре
        {
            moveDirection = Random.insideUnitCircle.normalized;
        }

        velocityMultiplier = Random.Range(2f, 5f);
        lifeDictance = Random.Range(minlifeDictance, maxlifeDictance);

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
        Vector2 toCentreDirection = (GameManager.Instance.CentreObject.transform.position - transform.position).normalized;
        return toCentreDirection * -1;
    }


    private IEnumerator LifeCycleEnumerator()
    {
        yield return new WaitUntil(() => (GameManager.Instance.CentreObject.transform.position - transform.position).magnitude >= lifeDictance);

        animatorBlinkingController.StartBlinking(false);
    }

    void IPooledObject.OnObjectSpawn()
    {
        InitializePlatform();
        StartCoroutine(LifeCycleEnumerator());
    }
}
