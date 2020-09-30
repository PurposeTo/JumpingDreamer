using UnityEngine;

public abstract class PlatformMovable : MonoBehaviour, IMovable
{
    private protected readonly int[] directionsToChoice = { -1, 1 };

    private protected Vector2 moveDirection;
    private protected float velocityMultiplier;

    private Vector2 velocity = Vector2.zero;
    public Vector2 Velocity => velocity;
    public event VelocityChanged OnVelocityChanged;

    private protected void SetVelocity(Vector2 newVelocity)
    {
        velocity = newVelocity;
        OnVelocityChanged?.Invoke();
    }
}
