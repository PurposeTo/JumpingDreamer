using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    private GameObject centre;
    private Rigidbody2D rb2D;

    private readonly float velocityMultiplier = 18f;

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

        Vector2 gravityProjectVector = GetGravityProjectVector();

        Vector2 finalVelocity = inputVelocity + gravityProjectVector;

        rb2D.velocity = finalVelocity;

        DrawForceRay(inputVelocityDirection, gravityProjectVector);
    }


    public Vector2 GetGravityProjectVector()
    {
        Vector2 toCentreDirection = ((Vector2)centre.transform.position - rb2D.position).normalized;
        return (Vector2)Vector3.Project(rb2D.velocity, toCentreDirection);
    }


    public void TossUp()
    {
        //float minGravityVelocity = 30f;
        //float bounciness = 0.05f;
        //float jumpScale = GravityProjectVector.magnitude < minGravityVelocity
        //    ? minGravityVelocity / GravityProjectVector.magnitude * bounciness
        //    : 1f;

        //Vector2 inputVelocity = rb2D.velocity - GravityProjectVector;
        //print($"КРЯ! rb2D.velocity: {rb2D.velocity}, GravityProjectVector: {GravityProjectVector}, inputVelocity: {inputVelocity}");


        //print($"КРЯ! jumpScale: {jumpScale}, GravityProjectVector: {GravityProjectVector.magnitude}");

        //print($"КРЯ! GravityProjectVector: {GravityProjectVector}");
        //GravityProjectVector *= -1;// * bounciness * jumpScale;
        //Vector2 finalVelocity = inputVelocity + GravityProjectVector;
        //print($"КРЯ! inputVelocity: {inputVelocity}, GravityProjectVector: {GravityProjectVector} finalVelocity: {finalVelocity}");

        //rb2D.velocity = finalVelocity;
    }


    private void DrawForceRay(Vector2 forceDirection, Vector2 gravityProjectVector)
    {
        Debug.DrawRay(transform.position, -forceDirection * 3, Color.yellow, 2f);

        Debug.DrawRay(transform.position, gravityProjectVector.normalized * 3, Color.green, 2f);
    }
}
