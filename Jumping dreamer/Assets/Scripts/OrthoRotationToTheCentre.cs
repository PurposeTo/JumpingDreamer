﻿using UnityEngine;

public class OrthoRotationToTheCentre : MonoBehaviour
{
    private GameObject centre;

    private void Start()
    {
        centre = GameManager.Instance.CentreObject;
    }

    private void Update()
    {
        transform.rotation = GameLogic.GetOrthoRotation(transform.position, centre.transform.position);
    }
}