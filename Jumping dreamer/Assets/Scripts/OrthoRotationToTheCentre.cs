using UnityEngine;

public class OrthoRotationToTheCentre : SuperMonoBehaviour
{
    private GameObject centre;

    protected override void StartWrapped()
    {
        centre = GameObjectsHolder.Instance.Centre.gameObject;
        SetOrthoRotation();
    }


    protected override void UpdateWrapped()
    {
        SetOrthoRotation();
    }


    private void SetOrthoRotation()
    {
        transform.rotation = GameLogic.GetOrthoRotation(transform.position, centre.transform.position);
    }
}
