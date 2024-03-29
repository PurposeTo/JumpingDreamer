﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlatformMovement : MonoBehaviour
{
    private Rigidbody2D rb2D;

    private IMovable[] IMovableArray;
    private List<Vector2> ListOfVelocity;
    private Vector2 finalVelocity = Vector2.zero;


    private void Start()
    {
        rb2D = gameObject.GetComponent<Rigidbody2D>();

        IMovableArray = gameObject.GetComponents<IMovable>();

        Array.ForEach(IMovableArray, IMovable => IMovable.OnVelocityChanged += UpdateFinalVelocity);

        UpdateFinalVelocity();
    }


    private void OnDestroy()
    {
        if (IMovableArray != null) Array.ForEach(IMovableArray, IMovable => IMovable.OnVelocityChanged -= UpdateFinalVelocity);
    }


    private void FixedUpdate()
    {
        rb2D.MovePosition(rb2D.position + (finalVelocity * Time.fixedDeltaTime));

        Debug.DrawRay(transform.position, finalVelocity.normalized * 3, Color.yellow, 2f);

    }


    private void UpdateFinalVelocity()
    {

        ListOfVelocity = IMovableArray.Select(x => x.Velocity).ToList();
        finalVelocity = ListOfVelocity.Aggregate((x, y) => x + y);
    }
}
