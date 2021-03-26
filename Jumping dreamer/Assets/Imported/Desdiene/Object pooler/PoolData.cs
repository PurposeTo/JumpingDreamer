using UnityEngine;

namespace Desdiene.Object_pooler
{

    [CreateAssetMenu(fileName = "PoolData", menuName = "ScriptableObjects/Pool")]
    public class PoolData : ScriptableObject
    {
        public GameObject prefab;
        public int size;
        public bool shouldExpand = true;
    }
}
