using UnityEngine;

public class OrthoRotationToTheCentre : SuperMonoBehaviour
{
    private GameObject Centre => GameObjectsHolder.Instance.Centre.gameObject;

    protected override void StartWrapped()
    {
        SetOrthoRotation();
    }


    protected override void UpdateWrapped()
    {
        SetOrthoRotation();
    }


    private void SetOrthoRotation()
    {
        transform.rotation = GameLogic.GetOrthoRotation(transform.position, Centre.transform.position);
    }
}
