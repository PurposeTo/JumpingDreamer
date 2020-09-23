using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private GameObject centre;
    private Rigidbody2D rb2D;

    private float velocityMultiplier = 18f;

    public Vector2 GravityProjectVector { get; private set; }
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
        Vector2 toCentreDirection = ((Vector2)centre.transform.position - rb2D.position).normalized;

        Vector2 inputVelocityDirection = GameLogic.GetOrthoNormalizedVector2(toCentreDirection);
        inputVelocityDirection *= HorizontalInput;
        inputVelocityDirection *= -1;

        Vector2 inputVelocity = inputVelocityDirection * velocityMultiplier;

        GravityProjectVector = (Vector2)Vector3.Project(rb2D.velocity, toCentreDirection);

        Vector2 finalVelocity = inputVelocity + GravityProjectVector;

        rb2D.velocity = finalVelocity;

        DrawForceRay(inputVelocityDirection, GravityProjectVector);
    }


    private void DrawForceRay(Vector2 forceDirection, Vector2 gravityProjectVector)
    {
        Debug.DrawRay(transform.position, -forceDirection * 3, Color.yellow, 2f);

        Debug.DrawRay(transform.position, gravityProjectVector.normalized * 3, Color.green, 2f);
    }
}
