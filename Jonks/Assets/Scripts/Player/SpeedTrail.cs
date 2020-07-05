using UnityEngine;

public class SpeedTrail : MonoBehaviour
{
    private Rigidbody2D rb2D;
    private TrailRenderer trailRenderer;

    private float velocityToEnableTrail = 25f;

    private void Start()
    {
        velocityToEnableTrail = ScoreCollector.VelocityToCollectScore;

        rb2D = GetComponent<Rigidbody2D>();
        trailRenderer = GetComponent<TrailRenderer>();

        trailRenderer.emitting = false;
    }

    private void Update()
    {
        if (rb2D.velocity.magnitude >= velocityToEnableTrail)
        {
            trailRenderer.emitting = true;
        }
        else
        {
            trailRenderer.emitting = false;
        }
    }
}
