using System.Collections;
using UnityEngine;

public class TrainingTutorial : MonoBehaviour
{
    private PlayerTactics playerTactics;
    private float AbsAverageHorizontalInput;
    private float minHorizontalInputToShowTutorial = 0.2f;
    private int minTotalLifeTimeToShowTutorial = 60;
    private float delay = 30f;

    private Coroutine ShowTutorialRoutine = null;
    private Coroutine CheckingIfTutorialNeedsToBeShownRoutine = null;


    private void Start()
    {
        playerTactics = GameManager.Instance.PlayerPresenter.PlayerTactics;

        bool shouldStartByShowingTheTutorial = PlayerStatsDataStorageSafe.Instance.PlayerStatsData.TotalLifeTime < minTotalLifeTimeToShowTutorial;

        if (CheckingIfTutorialNeedsToBeShownRoutine == null)
        {
            CheckingIfTutorialNeedsToBeShownRoutine = StartCoroutine(CheckingIfTutorialNeedsToBeShownEnumerator(shouldStartByShowingTheTutorial));
        }
    }


    private void Update()
    {
        AbsAverageHorizontalInput = playerTactics.AverageAbsVelocityDirection;
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
        while (IsTutorialNeedsToBeShown())
        {
            yield return null;
            Debug.Log("Обучаю играть в игру! Замени меня реализацией обучения!");
        }

        ShowTutorialRoutine = null;
    }
}
