using System;
using Desdiene.Container;
using Desdiene.SuperMonoBehaviourAsset;
using Desdiene.UnityEngineExtension;
using UnityEngine;

namespace Desdiene.ObjectPoolerAsset.Base
{
    internal class FromPoolSpawner : MonoBehaviourContainer
    {
        private readonly Pools pools;
        private readonly ObjectToPoolCreator objectCreator;


        public FromPoolSpawner(
            MonoBehaviour monoBehaviour, 
            Pools pools, 
            ObjectToPoolCreator objectCreator)
            : base(monoBehaviour)
        {
            this.pools = pools;
            this.objectCreator = objectCreator;
        }



        public void ReturnToPool(GameObject gameObject)
        {
            RecycleGameObject(gameObject);
            gameObject.SetActive(false);
        }


        public GameObject SpawnFromPool(GameObject prefabKey)
        {
            Pool pool = pools.Get(prefabKey);

            // Посмотреть на первый обьект в очереди.
            GameObject objectToSpawn = pool.ObjectPoolQueue.Peek();

            if (objectToSpawn.activeInHierarchy)
            {
                // Если объект включен (нельзя использовать)
                // И можно расширить пул
                if (pool.ShouldExpand)
                {
                    //То сделать новый объект
                    objectToSpawn = objectCreator.CreateNewObjectToPool(prefabKey, pool.PoolParent);
                }
            }
            else
            {
                // Если он выключен, то можно использовать. 
                objectToSpawn = pool.ObjectPoolQueue.Dequeue();
            }

            objectToSpawn.transform.SetDefault();
            RecycleGameObject(objectToSpawn);
            objectToSpawn.SetActive(true);
            OnObjectSpawn(objectToSpawn);

            pool.ObjectPoolQueue.Enqueue(objectToSpawn);

            return objectToSpawn;
        }


        private void OnObjectSpawn(GameObject objectToSpawn)
        {
            IPooledObject[] pooledComponents = objectToSpawn.GetComponents<IPooledObject>();

            Array.ForEach(pooledComponents, (pooled) =>
            {
                pooled.OnObjectSpawn();
            });
        }


        /// <summary>
        /// Подготавливает игровой объект к новому спавну.
        /// </summary>
        /// <param name="gameObject"></param>
        private void RecycleGameObject(GameObject gameObject)
        {
            var components = gameObject.GetComponentsInChildren<SuperMonoBehaviour>();
            Array.ForEach(components, component => component.BreakAllCoroutines());
        }

    }
}
