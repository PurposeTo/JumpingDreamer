using UnityEngine;

public class GravitySensitive : MonoBehaviour
{
    private float maxGravityProjectMagnitude = 50f;
    private float gravityProjectMagnitude = 0f;

    private PlayerMovement playerMovement;


    private void Start()
    {
        playerMovement = gameObject.GetComponent<PlayerMovement>();
    }


    private void FixedUpdate()
    {
        gravityProjectMagnitude = playerMovement.GetGravityProjectVector().magnitude;

        if (gravityProjectMagnitude > maxGravityProjectMagnitude) maxGravityProjectMagnitude = gravityProjectMagnitude;
    }


    public float GetGravitySensitive()
    {
        return gravityProjectMagnitude / maxGravityProjectMagnitude;
    }
}
