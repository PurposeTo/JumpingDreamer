using System;
using System.Collections.Generic;
using UnityEngine;
using Desdiene.Singleton;
using Desdiene.Super_monoBehaviour;

namespace Desdiene.Object_pooler
{

    public class ObjectPooler : SingletonSuperMonoBehaviour<ObjectPooler>
    {
        public List<PoolData> PoolDatas; // Сетим через инспектор

        private readonly Dictionary<GameObject, Pool> poolDictionary = new Dictionary<GameObject, Pool>();


        protected override void AwakeSingleton()
        {
            CreateAndInitializePools();
        }

        public new T SpawnFromPoolAndGetComponent<T>(GameObject prefabKey, Vector3 position, Transform parent = null)
        where T : Component
        {
            return Instance.SpawnFromPool(prefabKey, position, parent).GetComponent<T>();
        }


        public new T SpawnFromPoolAndGetComponent<T>(GameObject prefabKey, Vector3 position, Quaternion rotation, Transform parent = null)
        where T : Component
        {
            return Instance.SpawnFromPool(prefabKey, position, rotation, parent).GetComponent<T>();
        }


        public new GameObject SpawnFromPool(GameObject prefabKey, Vector3 position, Transform parent = null)
        {
            return SpawnFromPool(prefabKey, position, Quaternion.identity, parent);
        }


        public new GameObject SpawnFromPool(GameObject prefabKey, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (!poolDictionary.ContainsKey(prefabKey))
            {
                Debug.LogError($"Pool with prefabKey \"{prefabKey.name}\" does not exist");
                return null;
            }

            Pool pool = poolDictionary[prefabKey];


            // Посмотреть на первый обьект в очереди.
            GameObject objectToSpawn = pool.ObjectPoolQueue.Peek();

            if (objectToSpawn.activeInHierarchy)
            {
                // Если включен
                // И можно расширить пул
                if (pool.ShouldExpand)
                {
                    //То сделать новый объект
                    objectToSpawn = CreateNewObjectToPool(prefabKey, pool.PoolParent);
                }
            }
            else
            {
                // Если он выключен, то можно использовать. 
                objectToSpawn = pool.ObjectPoolQueue.Dequeue();
            }

            objectToSpawn.transform.position = position;
            objectToSpawn.transform.rotation = rotation;
            if (parent != null) { objectToSpawn.transform.SetParent(parent); }
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
                if (pooled is SuperMonoBehaviour superMonoBehaviour)
                {
                    superMonoBehaviour.BreakAllCoroutines();
                }

                pooled.OnObjectSpawn();
            });
        }


        private void CreateAndInitializePools()
        {
            foreach (var poolData in PoolDatas)
            {
                GameObject parent = CreateNewPoolParent(poolData.prefab.name);
                Queue<GameObject> objectPool = CreateNewPoolQueue(poolData, parent.transform);

                poolDictionary.Add(poolData.prefab, new Pool(poolData, objectPool, parent.transform));
                Debug.Log($"Pool with {poolData.prefab.name}s has been created!");
            }
        }


        private GameObject CreateNewObjectToPool(GameObject newGameObject, Transform poolParent)
        {
            newGameObject = Instantiate(newGameObject);
            newGameObject.transform.SetParent(poolParent);
            newGameObject.SetActive(false);

            return newGameObject;
        }


        private GameObject CreateNewPoolParent(string PrefabName)
        {
            GameObject parent = new GameObject(PrefabName + " Pool");
            parent.transform.SetParent(gameObject.transform);
            return parent;
        }


        private Queue<GameObject> CreateNewPoolQueue(PoolData poolData, Transform parentTransform)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < poolData.size; i++)
            {
                GameObject NewObjectToPool = CreateNewObjectToPool(poolData.prefab, parentTransform);
                objectPool.Enqueue(NewObjectToPool);
            }

            return objectPool;
        }
    }
}
