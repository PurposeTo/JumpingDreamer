using System.Collections.Generic;
using UnityEngine;
using Desdiene.Singleton;
using Desdiene.ObjectPoolerAsset.Base;

namespace Desdiene.ObjectPoolerAsset
{

    public class ObjectPooler : SingletonSuperMonoBehaviour<ObjectPooler>
    {
        public List<PoolData> PoolDatas; // Сетим через инспектор

        private FromPoolSpawner fromPoolSpawner;

        protected override void AwakeSingleton()
        {
            ObjectToPoolCreator objectCreator = new ObjectToPoolCreator();
            fromPoolSpawner = new FromPoolSpawner(
                this,
                new PoolCreator(this, objectCreator).CreateAndInitializePools(PoolDatas),
                objectCreator);
        }


        public void ReturnToPool(GameObject gameObject)
        {
            fromPoolSpawner.ReturnToPool(gameObject);
        }


        public GameObject SpawnFromPool(GameObject prefabKey)
        {
            return fromPoolSpawner.SpawnFromPool(prefabKey);
        }
    }
}
