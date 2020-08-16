using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTactics : MonoBehaviour
{
    const int fixedFramesInSecond = 50;

    public float PercentageOfTimeSpentByThePlayerMoving
    {
        get
        {
            return Mathf.Abs(totalInputPerSeconds) / fixedFramesInSecond / durationOfCollectingInformationAboutTactics;
        }
    }

    public float AverageAbsVelocityDirection
    {
        get
        {
            return totalAbsHorizontalInputPerSeconds / fixedFramesInSecond / durationOfCollectingInformationAboutTactics;
        }
    }

    private PlayerPresenter playerPresenter;

    private readonly float durationOfCollectingInformationAboutTactics = 18f;
    private Queue<float> horizontalInputQueue = new Queue<float>(); // Очередь необходима для ограничения записи значений
    private Queue<float> absHorizontalInputQueue = new Queue<float>(); // Очередь необходима для ограничения записи значений
    private float totalInputPerSeconds;
    private float totalAbsHorizontalInputPerSeconds;
    private float HorizontalInput => playerPresenter.PlayerMovement.HorizontalInput;
    private float AbsHorizontalInput => Mathf.Abs(playerPresenter.PlayerMovement.HorizontalInput);
    private float absHorizontalInput;


    private void Start()
    {
        playerPresenter = gameObject.GetComponent<PlayerPresenter>();
    }


    private void FixedUpdate()
    {
        absHorizontalInput = AbsHorizontalInput;
        UpdateHorizontalInputQueue();
        UpdateAbsHorizontalInputQueue();
    }


    private void UpdateHorizontalInputQueue()
    {
        if (horizontalInputQueue.Count >= durationOfCollectingInformationAboutTactics * fixedFramesInSecond)
        {
            float oldHorizontalInput = horizontalInputQueue.Dequeue();
            totalInputPerSeconds -= oldHorizontalInput;
        }

        float horizontalInput = HorizontalInput;
        totalInputPerSeconds += horizontalInput;
        horizontalInputQueue.Enqueue(horizontalInput);
    }


    private void UpdateAbsHorizontalInputQueue()
    {
        if (absHorizontalInputQueue.Count >= durationOfCollectingInformationAboutTactics * fixedFramesInSecond)
        {
            float oldHorizontalInput = absHorizontalInputQueue.Dequeue();
            totalAbsHorizontalInputPerSeconds -= oldHorizontalInput;
        }

        float absHorizontalInput = AbsHorizontalInput;
        totalAbsHorizontalInputPerSeconds += absHorizontalInput;
        absHorizontalInputQueue.Enqueue(absHorizontalInput);
    }
}
