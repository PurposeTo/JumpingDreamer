using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTactics : MonoBehaviour
{
    PlayerPresenter playerPresenter;

    private readonly float durationOfCollectingInformationAboutTactics = 10f;
    private Queue<float> horizontalInputQueue = new Queue<float>(); // Очередь необходима для ограничения записи значений
    private float totalInputPerSeconds;
    private float HorizontalInput => playerPresenter.PlayerMovement.HorizontalInput;


    private void Start()
    {
        playerPresenter = gameObject.GetComponent<PlayerPresenter>();
    }

    private void FixedUpdate()
    {
        if (horizontalInputQueue.Count >= durationOfCollectingInformationAboutTactics / Time.fixedDeltaTime)
        {
            float oldHorizontalInput = horizontalInputQueue.Dequeue();
            totalInputPerSeconds -= oldHorizontalInput;
        }

        float horizontalInput = HorizontalInput;
        totalInputPerSeconds += horizontalInput;
        horizontalInputQueue.Enqueue(horizontalInput);
    }


    public float GetPercentageOfTimeSpentByThePlayerMoving()
    {
        int fixedFramesInSecond = 50;
        return Mathf.Abs(totalInputPerSeconds) / fixedFramesInSecond / durationOfCollectingInformationAboutTactics;
    }
}
