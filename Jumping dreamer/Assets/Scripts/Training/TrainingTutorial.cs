using System;
using System.Collections;
using System.Linq;
using Desdiene.Coroutine.CoroutineExecutor;
using Desdiene.Super_monoBehaviour;
using UnityEngine;

public class TrainingTutorial : SuperMonoBehaviour
{
    public TrainingTipContainer[] trainingTips;
    private AnimatorByScript<FadeAnimation>[] fadeAnimators;
    private AnimatorByScript<BlinkingLoopAnimation>[] blinkingLoopAnimators;

    private PlayerTactics playerTactics;
    private float absAverageHorizontalInput;
    private readonly float minHorizontalInputToShowTutorial = 0.2f;
    private readonly int minTotalLifeTimeToShowTutorial = 60;
    private readonly float delay = 30f;

    private ICoroutineContainer CheckingIfTutorialNeedsToBeShownRoutineInfo;


    protected override void AwakeWrapped()
    {
        InitializeAnimations();
        playerTactics = GameObjectsHolder.Instance.PlayerPresenter.PlayerTactics;
        CheckingIfTutorialNeedsToBeShownRoutineInfo = CreateCoroutineContainer();
    }


    protected override void OnEnableWrapped()
    {
        PlayerDataModelController.InitializedInstance += (instance) =>
        {
            bool shouldStartByShowingTheTutorial = instance.DataInteraction.Getter.Stats.TotalLifeTime < minTotalLifeTimeToShowTutorial;

            Array.ForEach(trainingTips, trainingTip => trainingTip.gameObject.SetActive(false));

            if (IsTutorialNeedsToBeShown())
            {
                ReStartCoroutineExecution(ref CheckingIfTutorialNeedsToBeShownRoutineInfo,
                    CheckingIfTutorialNeedsToBeShownEnumerator(shouldStartByShowingTheTutorial));
            }
        };
    }


    protected override void UpdateWrapped()
    {
        absAverageHorizontalInput = playerTactics.AverageAbsVelocityDirection;
    }


    private void InitializeAnimations()
    {
        fadeAnimators = trainingTips.SelectMany(trainingTip =>
        {
            return new AnimatorByScript<FadeAnimation>[]
            {
                new AnimatorByScript<FadeAnimation>(
                    new FadeAnimation(this, trainingTip.GetTrainingTipObject().GetComponent<ImageRendererContainer>()),
                    this),

                new AnimatorByScript<FadeAnimation>(
                    new FadeAnimation(this, trainingTip.GetTrainingTextObject().GetComponent<TMProRendererContainer>()),
                    this),
            };
        }).ToArray();

        blinkingLoopAnimators = trainingTips.Select(trainingTip =>
        {
            return new AnimatorByScript<BlinkingLoopAnimation>(
                new BlinkingLoopAnimation(this, trainingTip.GetTrainingTipObject().GetComponent<ImageRendererContainer>()),
                this);
        }).ToArray();
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
            fadeAnimator.Animation.SetFadeState(FadeAnimation.FadeState.fadeIn);
            fadeAnimator.StartAnimation();
        });
        yield return new WaitWhile(() => fadeAnimators.Any(fadeAnimator => fadeAnimator.IsExecuting));

        Array.ForEach(blinkingLoopAnimators, (blinkingLoopAnimator) =>
        {
            blinkingLoopAnimator.Animation.SetLowerAlphaValue(0f);
            blinkingLoopAnimator.Animation.SetInfiniteNumberOfLoops();
            blinkingLoopAnimator.StartAnimation();
        });
    }


    private IEnumerator DisableTutorialBlinking()
    {
        Array.ForEach(blinkingLoopAnimators, (blinkingLoopAnimator) =>
        {
            blinkingLoopAnimator.Animation.SetLoopsCount(0);
        });
        yield return new WaitWhile(() => blinkingLoopAnimators.Any(blinkingLoopAnimator => blinkingLoopAnimator.IsExecuting));


        Array.ForEach(fadeAnimators, (fadeAnimator) =>
        {
            fadeAnimator.Animation.SetFadeState(FadeAnimation.FadeState.fadeOut);
            fadeAnimator.StartAnimation();
        });
        yield return new WaitWhile(() => fadeAnimators.Any(fadeAnimator => fadeAnimator.IsExecuting));
        Array.ForEach(trainingTips, trainingTip => trainingTip.gameObject.SetActive(false));
    }
}
