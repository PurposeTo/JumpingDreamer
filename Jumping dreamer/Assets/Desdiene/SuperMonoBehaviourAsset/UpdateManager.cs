using System;
using System.Collections.Generic;
using Desdiene.Singleton;

namespace Desdiene.SuperMonoBehaviourAsset
{
    [Obsolete("Не дает существенный прирост в текущих проектах")]
    public class UpdateManager : SingletonSuperMonoBehaviour<UpdateManager>
    {
        private static readonly List<Action> AllUpdates = new List<Action>();
        private static readonly List<Action> AllFixedUpdates = new List<Action>();
        private static readonly List<Action> AllLateUpdates = new List<Action>();


        private void Update()
        {
            for (int i = 0; i < AllUpdates.Count; i++)
            {
                AllUpdates[i]?.Invoke();
            }
        }


        private void FixedUpdate()
        {
            for (int i = 0; i < AllFixedUpdates.Count; i++)
            {
                AllFixedUpdates[i]?.Invoke();
            }
        }


        private void LateUpdate()
        {
            for (int i = 0; i < AllLateUpdates.Count; i++)
            {
                AllLateUpdates[i]?.Invoke();
            }
        }


        public static void AddUpdate(Action update) => AllUpdates.Add(update);
        public static void RemoveUpdate(Action update) => AllUpdates.Remove(update);

        public static void AddFixedUpdate(Action fixedUpdate) => AllFixedUpdates.Add(fixedUpdate);
        public static void RemoveFixedUpdate(Action fixedUpdate) => AllFixedUpdates.Remove(fixedUpdate);

        public static void AddLateUpdate(Action lateUpdate) => AllLateUpdates.Add(lateUpdate);
        public static void RemoveLateUpdate(Action lateUpdate) => AllLateUpdates.Remove(lateUpdate);
    }
}
