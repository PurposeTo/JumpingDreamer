using System;
using System.Collections;
using Desdiene.Coroutine.CoroutineExecutor;

namespace Desdiene.Coroutines.CoroutineExecutor
{
    public sealed class CoroutineWithData : ICoroutineContainer
    {
        public IEnumerator Enumerator { get; private set; } = null;
        public UnityEngine.Coroutine Coroutine { get; private set; } = null;
        public bool IsExecuting => Coroutine != null;


        public void SetCoroutine(UnityEngine.Coroutine coroutine)
        {
            Coroutine = coroutine ?? throw new ArgumentNullException(nameof(coroutine));
        }


        public void SetEnumerator(IEnumerator enumerator)
        {
            Enumerator = enumerator ?? throw new ArgumentNullException(nameof(enumerator));
        }


        /// <summary>
        /// Выполняется во время выполнении метода ExecuteCoroutineContinuously, 
        /// в случае, если корутина уже была запущена.
        /// </summary>
        public Action OnCoroutineAlreadyStarted { get; set; } = null;
        /// <summary>
        /// Выполняется во время выполнении метода BreakCoroutine,
        /// после остановки корутины.
        /// </summary>
        public Action OnStopCoroutine { get; set; } = null;
        /// <summary>
        /// Выполняется во время выполнении метода BreakCoroutine,
        /// в случае, если корутина УЖЕ была остановлена.
        /// </summary>
        public Action OnCoroutineIsAlreadyStopped { get; set; } = null;


        public void SetNullToCoroutine() => Coroutine = null;
    }

}
