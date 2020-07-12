using UnityEngine;

/// <summary>
/// Платформа при создании движется от центра
/// </summary>
public class MovementFromTheCenter : MovingPlatform, IMovable
{
    private float lifeDictance; // Максимальное расстояние от центра до точки, где платформы еще могут существовать
    private readonly float minlifeDictance = 50f;
    private readonly float maxlifeDictance = 80f;

    private bool isDisabling = false;

    [SerializeField]
    private bool UpdateMoveDirectionEveryFrame = false;


    private protected override void Start()
    {
        base.Start();
        InitializePlatform();
    }


    private void FixedUpdate()
    {
        CheckHeightForDisabling();

        if (UpdateMoveDirectionEveryFrame)
        {
            UpdateMoveDirection();
        }
    }


    /// <summary>
    /// Инициализирует платформу. Обращаться только после выполнения base.Start() !
    /// </summary>
    private void InitializePlatform()
    {
        isDisabling = false;

        // Если платформа не в центре
        if (transform.position != centre.transform.position)
        {
            Vector2 toCentreDirection = (centre.transform.position - transform.position).normalized;
            moveDirection = toCentreDirection * -1;
        }
        else // Если платформа в центре
        {
            moveDirection = Random.insideUnitCircle.normalized;
        }

        velocityMultiplier = Random.Range(2f, 5f);
        lifeDictance = Random.Range(minlifeDictance, maxlifeDictance);

        SetVelocity(moveDirection * velocityMultiplier);
    }


    private void CheckHeightForDisabling()
    {
        if ((centre.transform.position - transform.position).magnitude >= lifeDictance && !isDisabling)
        {
            // Множество раз включает триггер...
            animator.SetTrigger("StartBlinding");
            isDisabling = true;
        }
    }


    private void UpdateMoveDirection()
    {
        if (transform.position != centre.transform.position)
        {
            Vector2 toCentreDirection = (centre.transform.position - transform.position).normalized;
            moveDirection = toCentreDirection * -1;
        }
    }
}
