using System;
using UnityEngine;
using Desdiene.SuperMonoBehaviourAsset;

namespace Desdiene.Singleton
{

    /// <summary> 
    /// To access the heir by a static field "Instance".
    /// </summary>
    public abstract class SingletonSuperMonoBehaviour<T> : SuperMonoBehaviour where T : SingletonSuperMonoBehaviour<T>
    {
        [SerializeField] private bool dontDestroyOnLoad = false;

        public static T Instance { get; private set; }


        /// <summary>
        /// Данное событие выполнится тогда, когда Instance будет инициализирован.
        /// Команды выполняются сразу в Awake() после метода AwakeSingleton(), если синглтон не был инициализирован.
        /// </summary>
        public static event Action<T> InitializedInstance
        {
            add
            {
                if (Instance != null) value?.Invoke(Instance);
                else OnInstanceInitialize += value;
            }
            remove
            {
                OnInstanceInitialize -= value;
            }
        }

        private static Action<T> OnInstanceInitialize;


        protected sealed override void AwakeWrapped()
        {
            if (Instance == null)
            {
                Debug.Log($"Initialize SingletonSuperMonoBehaviour {this}");
                Instance = this as T;
                if (dontDestroyOnLoad)
                {
                    DontDestroyOnLoad(gameObject);
                }
                AwakeSingleton();

                ExecuteCommandsAndClear();
            }
            else
            {
                Destroy(gameObject); //Destroy(gameObject.GetComponent<T>());
            }
        }


        /// <summary>
        /// Используется после инициализации Singleton. Использовать вместо Awake/AwakeWrapped.
        /// </summary>
        protected virtual void AwakeSingleton() { }


        private void ExecuteCommandsAndClear()
        {
            OnInstanceInitialize?.Invoke(Instance);
            OnInstanceInitialize = null;
        }
    }

    /*Пример с GameManager
     * 
     *  public class GameManager : Singleton<GameManager>
    {
        protected override void AwakeSingleton() { }
    }
     */
}
