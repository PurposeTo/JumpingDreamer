using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class TrainingTutorial : SuperMonoBehaviour
{
    public TrainingTipContainer[] trainingTips;
    private FadeAnimator[] fadeAnimators;
    private BlinkingLoopAnimator[] blinkingLoopAnimators;

    private PlayerTactics playerTactics;
    private float absAverageHorizontalInput;
    private readonly float minHorizontalInputToShowTutorial = 0.2f;
    private readonly int minTotalLifeTimeToShowTutorial = 60;
    private readonly float delay = 30f;

    private ICoroutineInfo CheckingIfTutorialNeedsToBeShownRoutineInfo;


    protected override void AwakeWrapped()
    {
        InitializeAnimations();
        playerTactics = GameObjectsHolder.Instance.PlayerPresenter.PlayerTactics;
        CheckingIfTutorialNeedsToBeShownRoutineInfo = CreateCoroutineInfo();
    }


    protected override void OnEnableWrapped()
    {
        bool shouldStartByShowingTheTutorial = PlayerDataModelController.Instance.GetPlayerDataModel().PlayerStats.TotalLifeTime < minTotalLifeTimeToShowTutorial;

        if (IsTutorialNeedsToBeShown())
        {
            ContiniousCoroutineExecution(ref CheckingIfTutorialNeedsToBeShownRoutineInfo,
                CheckingIfTutorialNeedsToBeShownEnumerator(shouldStartByShowingTheTutorial));
        }
    }


    protected override void UpdateWrapped()
    {
        absAverageHorizontalInput = playerTactics.AverageAbsVelocityDirection;
    }


    private void InitializeAnimations()
    {
        fadeAnimators = trainingTips.SelectMany(trainingTip =>
        {
            return new FadeAnimator[]
            {
                new FadeAnimator(this, trainingTip.GetTrainingTipObject().GetComponent<ImageRendererContainer>()),
                new FadeAnimator(this, trainingTip.GetTrainingTextObject().GetComponent<TMProRendererContainer>())
            };
        }).ToArray();

        blinkingLoopAnimators = trainingTips.Select(trainingTip =>
        new BlinkingLoopAnimator(this, trainingTip.GetTrainingTipObject().GetComponent<ImageRendererContainer>())).ToArray();
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
        yield return EnableTutorial();
        yield return new WaitWhile(() => IsTutorialNeedsToBeShown());
        yield return DisableTutorialBlinking();
    }


    private IEnumerator EnableTutorial()
    {
        Array.ForEach(trainingTips, trainingTip => trainingTip.gameObject.SetActive(true));
        Array.ForEach(fadeAnimators, (fadeAnimator) =>
        {
            fadeAnimator.SetFadeState(FadeAnimator.FadeState.fadeIn);
            fadeAnimator.StartAnimation();
        });
        yield return new WaitWhile(() => fadeAnimators.Any(fadeAnimator => fadeAnimator.IsExecuting));

        Array.ForEach(blinkingLoopAnimators, (blinkingLoopAnimator) =>
        {
            blinkingLoopAnimator.SetLowerAlphaValue(0f);
            blinkingLoopAnimator.SetInfiniteNumberOfLoops();
            blinkingLoopAnimator.StartAnimation();
        });
    }


    private IEnumerator DisableTutorialBlinking()
    {
        Array.ForEach(blinkingLoopAnimators, (blinkingLoopAnimator) =>
        {
            blinkingLoopAnimator.SetLoopsCount(0);
        });
        yield return new WaitWhile(() => blinkingLoopAnimators.Any(blinkingLoopAnimator => blinkingLoopAnimator.IsExecuting));


        Array.ForEach(fadeAnimators, (fadeAnimator) =>
        {
            fadeAnimator.SetFadeState(FadeAnimator.FadeState.fadeOut);
            fadeAnimator.StartAnimation();
        });
        yield return new WaitWhile(() =>  fadeAnimators.Any(fadeAnimator => fadeAnimator.IsExecuting));
        Array.ForEach(trainingTips, trainingTip => trainingTip.gameObject.SetActive(false));
    }
}
