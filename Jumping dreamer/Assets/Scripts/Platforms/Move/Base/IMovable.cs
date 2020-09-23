using UnityEngine;

public delegate void VelocityChanged();
public interface IMovable
{
    Vector2 Velocity { get; }

    event VelocityChanged OnVelocityChanged;
}
