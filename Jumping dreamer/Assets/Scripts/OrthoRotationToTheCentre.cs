using Desdiene.SuperMonoBehaviourAsset;
using UnityEngine;

public class OrthoRotationToTheCentre : SuperMonoBehaviour
{
    private GameObject Centre => GameObjectsHolder.Instance.Centre.gameObject;

    protected override void StartWrapped()
    {
        SetOrthoRotation();
    }


    private void Update()
    {
        SetOrthoRotation();
    }


    private void SetOrthoRotation()
    {
        transform.rotation = GameLogic.GetOrthoRotation(transform.position, Centre.transform.position);
    }
}
