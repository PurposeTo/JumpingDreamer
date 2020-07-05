using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private GameObject centre;
    private Rigidbody2D rb2D;

    private float velocityMultiplier = 18f;

    private float horizontalInput;

    private Controller controller;


    private void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        centre = GameManager.Instance.Centre;

        controller = ControllerInitializer.InitializeController(Application.platform);
    }


    private void Update()
    {
        horizontalInput = controller.HorizontalInput;

        //print($"horizontalInput = {horizontalInput}");
    }


    private void FixedUpdate()
    {
        MoveCharacter();
    }


    private void MoveCharacter()
    {
        Vector2 toCentreDirection = ((Vector2)centre.transform.position - rb2D.position).normalized;

        Vector2 inputVelocityDirection = GameLogic.GetOrthoNormalizedVector2(toCentreDirection);
        inputVelocityDirection *= horizontalInput;
        inputVelocityDirection = -inputVelocityDirection;

        Vector2 inputVelocity = inputVelocityDirection * velocityMultiplier;

        Vector2 gravityProjectVector = (Vector2)Vector3.Project(rb2D.velocity, toCentreDirection);

        Vector2 finalVelocity = inputVelocity + gravityProjectVector;

        rb2D.velocity = finalVelocity;

        DrawForceRay(inputVelocityDirection, gravityProjectVector);
    }


    private void DrawForceRay(Vector2 forceDirection, Vector2 gravityProjectVector)
    {
        Debug.DrawRay(transform.position, -forceDirection * 3, Color.yellow, 2f);

        Debug.DrawRay(transform.position, gravityProjectVector.normalized * 3, Color.green, 2f);
    }
}
