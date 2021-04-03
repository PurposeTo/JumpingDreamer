using UnityEngine;

namespace Desdiene.ObjectPoolerAsset
{
    public static class ObjectPoolerExtension
    {
        /// <summary>
        /// Заспавнить игровой объект из пула
        /// </summary>
        /// <param name="prefabKey">Префаб, по которому будет найден игровой объект в пуле</param>
        /// <returns></returns>
        public static GameObject SpawnFromPool(this GameObject prefabKey)
        {
            GameObject pooled = ObjectPooler.Instance.SpawnFromPool(prefabKey);
            return pooled;
        }


        /// <summary>
        /// Заспавнить игровой объект из пула и получить компонент
        /// </summary>
        /// <typeparam name="T">Компонент, который будет получен с объекта</typeparam>
        /// <param name="prefabKey">Префаб, по которому будет найден игровой объект в пуле</param>
        /// <returns></returns>
        public static T SpawnFromPool<T>(this GameObject prefabKey) where T : Component
        {
            return ObjectPooler.Instance.SpawnFromPool(prefabKey).GetComponent<T>();
        }

        /// <summary>
        /// Выключить и вернуть объект в пул
        /// </summary>
        /// <param name="gameObject"></param>
        public static void ReturnToPool(this GameObject gameObject)
        {
            ObjectPooler.Instance.ReturnToPool(gameObject);
        }
    }
}
