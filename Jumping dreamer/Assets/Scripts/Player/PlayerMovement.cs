using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private GameObject centre;
    private Rigidbody2D rb2D;

    private readonly float velocityMultiplier = 18f;
    private readonly float minGravityVelocity = 30f;
    private readonly float bounciness = 0.8f;

    public Vector2 GravityProjectVector { get; private set; }
    public Vector2 InputVelocity { get; private set; }
    public float HorizontalInput { get; private set; }

    private Controller controller;


    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();
        centre = GameManager.Instance.CentreObject;

        controller = ControllerInitializer.InitializeController(Application.platform);
    }


    private void Update()
    {
        HorizontalInput = controller.HorizontalInput;

        //print($"horizontalInput = {horizontalInput}");
    }


    private void FixedUpdate()
    {
        MoveCharacter();
    }


    private void MoveCharacter()
    {
        Vector2 toCentreDirection = GameManager.Instance.GetToCentreDirection(rb2D.position);
        GravityProjectVector = GetGravityProjectVector(toCentreDirection);
        InputVelocity = GetInputVelocity(toCentreDirection);

        SetPlayerVelocity();
        DrawForceRay(InputVelocity, GravityProjectVector);
    }


    private Vector2 GetGravityProjectVector(Vector2 toCentreDirection)
    {
        return (Vector2)Vector3.Project(rb2D.velocity, toCentreDirection);
    }


    private Vector2 GetInputVelocity(Vector2 toCentreDirection)
    {
        Vector2 inputVelocityDirection = GameLogic.GetOrthoNormalizedVector2(toCentreDirection);
        inputVelocityDirection *= HorizontalInput;
        inputVelocityDirection *= -1;

        return inputVelocityDirection * velocityMultiplier;
    }


    private void SetPlayerVelocity()
    {
        rb2D.velocity = InputVelocity + GravityProjectVector;
    }


    public void TossUp(bool onlyUp)
    {
        if (onlyUp) // Подбросить только вверх
        {
            GravityProjectVector = GameManager.Instance.GetToCentreDirection(transform.position) * -1 * GravityProjectVector.magnitude * bounciness;
        }
        else // Отразить скорость
        {
            GravityProjectVector *= -1 * bounciness;
        }


        GravityProjectVector = GameLogic.ClampVectorByMagnitude(GravityProjectVector, minGravityVelocity);
        SetPlayerVelocity();
    }


    private void DrawForceRay(Vector2 forceDirection, Vector2 gravityProjectVector)
    {
        Debug.DrawRay(transform.position, -forceDirection.normalized * 3, Color.yellow, 2f);

        Debug.DrawRay(transform.position, gravityProjectVector.normalized * 3, Color.green, 2f);
    }
}
