using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class TrainingTutorial : SuperMonoBehaviour
{
    public GameObject[] trainingTips;
    private AnimatorBlinkingController[] animatorBlinkingControllers;

    private PlayerTactics playerTactics;
    private float absAverageHorizontalInput;
    private readonly float minHorizontalInputToShowTutorial = 0.2f;
    private readonly int minTotalLifeTimeToShowTutorial = 60;
    private readonly float delay = 30f;
    private bool isTutorialTipsEnable = false;

    private readonly float blinkingAnimationSpeed = 1.25f;

    private ICoroutineInfo CheckingIfTutorialNeedsToBeShownRoutineInfo;

    protected override void AwakeWrapped()
    {
        playerTactics = GameObjectsHolder.Instance.PlayerPresenter.PlayerTactics;
        animatorBlinkingControllers = trainingTips.Select(x => x.GetComponentInChildren<AnimatorBlinkingController>()).ToArray();
        animatorBlinkingControllers[0].OnDisableBlinking += DisableTutorialTips;
        CheckingIfTutorialNeedsToBeShownRoutineInfo = CreateCoroutineInfo();
    }


    private void OnEnable()
    {
        bool shouldStartByShowingTheTutorial = PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.TotalLifeTime < minTotalLifeTimeToShowTutorial;

        if (IsTutorialNeedsToBeShown())
        {
            ContiniousCoroutineExecution(ref CheckingIfTutorialNeedsToBeShownRoutineInfo,
                CheckingIfTutorialNeedsToBeShownEnumerator(shouldStartByShowingTheTutorial));
        }
    }


    private void OnDestroy()
    {
        animatorBlinkingControllers[0].OnDisableBlinking -= DisableTutorialTips;
    }


    private void OnDisable()
    {
        DisableTutorialTips();
        isTutorialTipsEnable = false;
    }


    private void Update()
    {
        absAverageHorizontalInput = playerTactics.AverageAbsVelocityDirection;
    }


    private void EnableTutorial()
    {
        Array.ForEach(trainingTips, gameObject => gameObject.SetActive(true));
        Array.ForEach(animatorBlinkingControllers, (x) =>
        {
            x.AwakeInititialized += () =>
            {
                x.SetBlinkingAnimationSpeed(blinkingAnimationSpeed);
                x.StartBlinking(false);
            };
        });

        isTutorialTipsEnable = true;
    }


    private void DisableTutorialBlinking()
    {
        Array.ForEach(animatorBlinkingControllers, (x) =>
        {
            x.AwakeInititialized += () =>
            {
                x.StopBlinking();
            };
        });
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
        return absAverageHorizontalInput <= minHorizontalInputToShowTutorial;
    }

    private IEnumerator CheckingIfTutorialNeedsToBeShownEnumerator(bool shouldStartByShowingTheTutorial)
    {
        if (shouldStartByShowingTheTutorial)
        {
            yield return ShowTrainingEnumerator();
        }

        WaitForSeconds wait = new WaitForSeconds(delay);

        while (true)
        {
            yield return wait;

            if (IsTutorialNeedsToBeShown())
            {
                yield return ShowTrainingEnumerator();
            }
        }
    }


    private IEnumerator ShowTrainingEnumerator()
    {
        EnableTutorial();
        yield return new WaitWhile(() => IsTutorialNeedsToBeShown());
        DisableTutorialBlinking();

        yield return new WaitWhile(() => isTutorialTipsEnable);
    }
}
