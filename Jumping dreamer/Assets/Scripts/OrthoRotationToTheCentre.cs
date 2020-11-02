using UnityEngine;

public class OrthoRotationToTheCentre : MonoBehaviour
{
    private GameObject centre;

    private void Start()
    {
        centre = GameObjectsHolder.Instance.Centre.gameObject;
        SetOrthoRotation();
    }


    private void Update()
    {
        SetOrthoRotation();
    }


    private void SetOrthoRotation()
    {
        transform.rotation = GameLogic.GetOrthoRotation(transform.position, centre.transform.position);
    }
}
