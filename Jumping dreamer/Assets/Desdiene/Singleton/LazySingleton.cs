using UnityEngine;

namespace Desdiene.Singleton
{
    public class LazySingleton<T> where T : LazySingleton<T>, new()
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new T();
                    Debug.Log($"{typeof(T)} was initialized by lazy initialization.");
                }
                return instance;
            }
        }


        // ВНИМАНИЕ!!! При сериализации класса Unity создает экземляр сериализуемого класса, например, при заходе в Editor, при включении сцены и т.п. Таким образом не понятно что происходит с instance.
        public LazySingleton()
        {
            if (instance == null)
            {
                instance = this as T;
                Debug.Log($"{typeof(T)} had been created and instance was initialized.");
            }
            else
            {
                Debug.LogWarning($"{typeof(T)} created. But singleton has already initialized.");
            }
        }
    }
}
