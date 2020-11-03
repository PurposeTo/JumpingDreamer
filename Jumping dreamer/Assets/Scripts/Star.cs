﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Animator))]
public class Star : MonoBehaviour
{
    private protected Animator animator;

    private float lifeTime;
    private readonly float minLifeTime = 10f;
    private readonly float maxLifeTime = 30f;

    private Coroutine lifeCoroutine = null;


    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void OnEnable()
    {
        void IncrementNumberOfActiveStars() => StarGenerator.Instance.NumberOfActiveStars++;
        StarGenerator.SetAwakeCommand(IncrementNumberOfActiveStars);

        lifeTime = Random.Range(minLifeTime, maxLifeTime);

        if (lifeCoroutine == null)
        {
            lifeCoroutine = StartCoroutine(LifeEnumerator());
        }
    }


    private void OnDisable()
    {
        void DecrementNumberOfActiveStars() => StarGenerator.Instance.NumberOfActiveStars--;
        StarGenerator.SetAwakeCommand(DecrementNumberOfActiveStars);

        if (lifeCoroutine != null)
        {
            StopCoroutine(lifeCoroutine);
            lifeCoroutine = null;
        }
    }


    private IEnumerator LifeEnumerator()
    {
        yield return new WaitForSeconds(lifeTime);
        animator.SetBool("isBlinding", true);

        lifeCoroutine = null;
    }


    public void DisableObject()
    {
        gameObject.SetActive(false);
    }
}
