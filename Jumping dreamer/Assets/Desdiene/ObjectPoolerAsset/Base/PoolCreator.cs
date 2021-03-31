using System.Collections.Generic;
using Desdiene.Container;
using UnityEngine;

namespace Desdiene.ObjectPoolerAsset.Base
{
    internal class PoolCreator : MonoBehaviourContainer
    {
        private readonly ObjectToPoolCreator objectCreator;

        public PoolCreator(MonoBehaviour monoBehaviour, ObjectToPoolCreator objectCreator)
            : base(monoBehaviour)
        {
            this.objectCreator = objectCreator;
        }

        public Pools CreateAndInitializePools(List<PoolData> PoolDatas)
        {
            Pools pools = new Pools();

            foreach (var poolData in PoolDatas)
            {
                GameObject parent = CreateNewPoolParent(poolData.prefab.name);
                Queue<GameObject> objectPool = CreateNewPoolQueue(poolData, parent.transform);

                pools.Add(poolData.prefab, new Pool(poolData, objectPool, parent.transform));
                Debug.Log($"Pool with {poolData.prefab.name}s has been created!");
            }

            return pools;
        }


        private GameObject CreateNewPoolParent(string PrefabName)
        {
            GameObject parent = new GameObject(PrefabName + " Pool");
            parent.transform.SetParent(monoBehaviour.gameObject.transform);
            return parent;
        }


        private Queue<GameObject> CreateNewPoolQueue(PoolData poolData, Transform parentTransform)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            for (int i = 0; i < poolData.size; i++)
            {
                GameObject NewObjectToPool = objectCreator.CreateNewObjectToPool(poolData.prefab, parentTransform);
                objectPool.Enqueue(NewObjectToPool);
            }

            return objectPool;
        }
    }
}
