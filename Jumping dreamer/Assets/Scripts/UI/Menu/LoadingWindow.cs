using Desdiene.Object_pooler;
using UnityEngine;

public class LoadingWindow : MonoBehaviour, IPooledObject
{
    public void TurnOff() => gameObject.SetActive(false);


    public void OnObjectSpawn() { }
}
