using System;
using UnityEngine;

public class GameObjectsHolder : SingletonSuperMonoBehaviour<GameObjectsHolder>
{
    [SerializeField] private PlayerPresenter playerPresenter = null;
    [SerializeField] private Centre centre = null;
    [SerializeField] private GameObject cameraObject = null;

    public PlayerPresenter PlayerPresenter { get => playerPresenter; }
    public Centre Centre { get => centre; }
    public GameObject CameraObject { get => cameraObject; }


    protected override void AwakeSingleton()
    {
        if (playerPresenter == null) throw new ArgumentNullException("playerPresenter");
        if (centre == null) throw new ArgumentNullException("centre");
        if (cameraObject == null) throw new ArgumentNullException("cameraObject");
    }
}
