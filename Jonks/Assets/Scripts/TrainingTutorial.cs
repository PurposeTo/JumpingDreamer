﻿using System.Collections;
using System.Linq;
using UnityEngine;

public class TrainingTutorial : MonoBehaviour
{
    public GameObject[] trainingTips;
    private AnimatorBlinkingController[] animatorBlinkingControllers;

    private PlayerTactics playerTactics;
    private float AbsAverageHorizontalInput;
    private float minHorizontalInputToShowTutorial = 0.2f;
    private int minTotalLifeTimeToShowTutorial = 60;
    private float delay = 30f;
    private bool isTutorialTipsEnable = false;

    private float blinkingAnimationSpeed = 1.25f;

    private Coroutine ShowTutorialRoutine = null;
    private Coroutine CheckingIfTutorialNeedsToBeShownRoutine = null;


    private void Awake()
    {
        playerTactics = GameManager.Instance.PlayerPresenter.PlayerTactics;
        animatorBlinkingControllers = trainingTips.Select(x => x.GetComponentInChildren<AnimatorBlinkingController>()).ToArray();
    }


    private void OnEnable()
    {
        bool shouldStartByShowingTheTutorial = PlayerStatsDataStorageSafe.Instance.PlayerStatsData.TotalLifeTime < minTotalLifeTimeToShowTutorial;

        if (CheckingIfTutorialNeedsToBeShownRoutine == null)
        {
            CheckingIfTutorialNeedsToBeShownRoutine = StartCoroutine(CheckingIfTutorialNeedsToBeShownEnumerator(shouldStartByShowingTheTutorial));
        }
    }


    private void OnDisable()
    {
        DisableTutorialBlinking();
        DisableTutorialTips();
        animatorBlinkingControllers[0].OnDisableBlinking -= DisableTutorialTips;
        StopAllCoroutines();
        ShowTutorialRoutine = null;
        CheckingIfTutorialNeedsToBeShownRoutine = null;
    }


    private void Update()
    {
        AbsAverageHorizontalInput = playerTactics.AverageAbsVelocityDirection;
    }


    private void EnableTutorial()
    {
        for (int i = 0; i < trainingTips.Length; i++)
        {
            trainingTips[i].SetActive(true);
        }

        for (int i = 0; i < animatorBlinkingControllers.Length; i++)
        {
            animatorBlinkingControllers[i].SetBlinkingAnimationSpeed(blinkingAnimationSpeed);
            animatorBlinkingControllers[i].StartBlinking(false);
        }

        isTutorialTipsEnable = true;
    }


    private void DisableTutorialBlinking()
    {
        for (int i = 0; i < animatorBlinkingControllers.Length; i++)
        {
            animatorBlinkingControllers[i].StopBlinking();
        }
    }

    private void DisableTutorialTips()
    {
        for (int i = 0; i < trainingTips.Length; i++)
        {
            trainingTips[i].SetActive(false);
        }

        isTutorialTipsEnable = false;
    }


    private bool IsTutorialNeedsToBeShown()
    {
        return AbsAverageHorizontalInput <= minHorizontalInputToShowTutorial;
    }


    private Coroutine StartToShowTutorial()
    {
        if (ShowTutorialRoutine == null)
        {
            ShowTutorialRoutine = StartCoroutine(ShowTrainingEnumerator());
            return ShowTutorialRoutine;
        }
        else return null;
    }


    private IEnumerator CheckingIfTutorialNeedsToBeShownEnumerator(bool shouldStartByShowingTheTutorial)
    {
        if (shouldStartByShowingTheTutorial)
        {
            yield return StartToShowTutorial();
        }

        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;

            if (IsTutorialNeedsToBeShown())
            {
                yield return StartToShowTutorial();
            }
        }
    }


    private IEnumerator ShowTrainingEnumerator()
    {
        EnableTutorial();
        yield return new WaitWhile(() => IsTutorialNeedsToBeShown());
        DisableTutorialBlinking();

        animatorBlinkingControllers[0].OnDisableBlinking += DisableTutorialTips;

        yield return new WaitWhile(() => isTutorialTipsEnable);

        animatorBlinkingControllers[0].OnDisableBlinking -= DisableTutorialTips;

        ShowTutorialRoutine = null;
    }
}
