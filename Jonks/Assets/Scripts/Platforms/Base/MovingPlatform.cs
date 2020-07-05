using UnityEngine;

public class MovingPlatform : BasePlatform, IMovable
{
    private protected Vector2 moveDirection;
    private protected float velocityMultiplier;

    private Vector2 velocity = Vector2.zero;
    public Vector2 Velocity => velocity;
    public VelocityChanged OnVelocityChanged { get; set; }

    private protected void SetVelocity(Vector2 newVelocity)
    {
        velocity = newVelocity;
        OnVelocityChanged?.Invoke();
    }
}
