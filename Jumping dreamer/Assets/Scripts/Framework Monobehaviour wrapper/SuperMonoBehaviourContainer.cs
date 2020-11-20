using System;

/// <summary>
/// Содержит поле superMonoBehaviour, которое инициализируется конструктором
/// </summary>
public class SuperMonoBehaviourContainer
{
    protected readonly SuperMonoBehaviour superMonoBehaviour;

    public SuperMonoBehaviourContainer(SuperMonoBehaviour superMonoBehaviour)
    {
        this.superMonoBehaviour = superMonoBehaviour != null ? superMonoBehaviour : throw new ArgumentNullException(nameof(superMonoBehaviour));
    }
}
