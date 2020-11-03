using System;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : SingletonMonoBehaviour<ObjectPooler>
{
    [System.Serializable]
    public class Pool
    {
        public GameObject prefab;
        public int size;
        public bool shouldExpand = true;
        public Transform PoolParent { get; set; }
        public Queue<GameObject> ObjectPoolQueue { get; set; }
    }

    public List<Pool> pools;

    public Dictionary<GameObject, Pool> poolDictionary = new Dictionary<GameObject, Pool>();


    protected override void AwakeSingleton()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            GameObject parent = new GameObject(pools[i].prefab.name + " Pool");

            pools[i].PoolParent = parent.transform;
            pools[i].PoolParent.SetParent(gameObject.transform);


            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int j = 0; j < pools[i].size; j++)
            {
                GameObject newGameObject = CreateNewObjectToPool(pools[i].prefab, pools[i].PoolParent);
                objectPool.Enqueue(newGameObject);
            }

            pools[i].ObjectPoolQueue = objectPool;

            poolDictionary.Add(pools[i].prefab, pools[i]);
            Debug.Log($"Pool with {pools[i].prefab.name}s has been created!");
        }
    }


    public void DisableAllObjects()
    {
        for (int i = 0; i < pools.Count; i++)
        {
            for (int j = 0; j < pools[i].ObjectPoolQueue.Count; j++)
            {
                GameObject objectToDisable = pools[i].ObjectPoolQueue.Dequeue();

                objectToDisable.SetActive(false);
                // Todo: Так же необходимо выключать все корутины на обьекте
                pools[i].ObjectPoolQueue.Enqueue(objectToDisable);
            }
        }
    }


    public GameObject SpawnFromPool(GameObject prefabKey, Vector3 position, Quaternion rotation, Transform parent = null)
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
            if (pool.shouldExpand)
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

        IPooledObject[] pooledComponents = objectToSpawn.GetComponents<IPooledObject>();
        Array.ForEach(pooledComponents, pooledComponent => pooledComponent.OnObjectSpawn());

        //if (objectToSpawn.TryGetComponent(out IPooledObject pooledObject)) pooledObject.OnObjectSpawn(); Если на объекте несколько таких интерфейсов, то будет вызван лишь один

        pool.ObjectPoolQueue.Enqueue(objectToSpawn);

        return objectToSpawn;
    }


    private GameObject CreateNewObjectToPool(GameObject newGameObject, Transform poolParent)
    {
        newGameObject = Instantiate(newGameObject);
        newGameObject.transform.SetParent(poolParent);
        newGameObject.SetActive(false);

        return newGameObject;
    }
}
