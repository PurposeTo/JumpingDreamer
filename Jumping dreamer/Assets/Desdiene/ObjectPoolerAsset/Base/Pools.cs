using System.Collections.Generic;
using UnityEngine;

namespace Desdiene.ObjectPoolerAsset.Base
{
    internal class Pools
    {
        private readonly Dictionary<GameObject, Pool> poolDictionary = new Dictionary<GameObject, Pool>();

        public void Add(GameObject prefabKey, Pool pool)
        {
            poolDictionary.Add(prefabKey, pool);
        }

        public Pool Get(GameObject prefabKey)
        {
            if (!ContainsPool(prefabKey))
            {
                Debug.LogError($"Pool with prefabKey \"{prefabKey.name}\" does not exist");
                return null;
            }

            return poolDictionary[prefabKey];
        }


        private bool ContainsPool(GameObject prefabKey)
        {
            return poolDictionary.ContainsKey(prefabKey);
        }
    }
}
