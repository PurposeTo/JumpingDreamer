﻿using Desdiene.ObjectPoolerAsset;
using UnityEngine;

public class LoadingWindow : MonoBehaviour, IPooledObject
{
    public void TurnOff() => gameObject.SetActive(false);


    public void OnObjectSpawn() { }
}
