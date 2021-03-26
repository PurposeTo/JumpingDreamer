using System;
using UnityEngine;

namespace Desdiene.Container
{
    /// <summary>
    /// Содержит поле monoBehaviour, которое инициализируется конструктором
    /// </summary>
    public abstract class MonoBehaviourContainer
    {
        protected readonly MonoBehaviour monoBehaviour;

        public MonoBehaviourContainer(MonoBehaviour monoBehaviour)
        {
            this.monoBehaviour = monoBehaviour != null ? monoBehaviour : throw new ArgumentNullException(nameof(monoBehaviour));
        }
    }
}
