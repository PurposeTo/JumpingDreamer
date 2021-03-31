using UnityEngine;

namespace Desdiene.ObjectPoolerAsset.Base
{
    internal class ObjectToPoolCreator
    {
        public GameObject CreateNewObjectToPool(GameObject newGameObject, Transform poolParent)
        {
            newGameObject = Object.Instantiate(newGameObject);
            newGameObject.transform.SetParent(poolParent);
            newGameObject.SetActive(false);

            return newGameObject;
        }
    }
}
