using System.Collections;
using UnityEngine;

public class WorldGenerationRulesController : SingletonMonoBehaviour<WorldGenerationRulesController>
{
    public PlatformGeneratorPresenter PlatformGeneratorPresenter { get; private set; }
    public ColorSchemePresenter ColorSchemePresenter { get; private set; }
    public FlashGeneratorPresenter FlashGeneratorPresenter { get; private set; }

    private readonly float timePeriodForTheGenerationRules = 35f;
    private Coroutine lifeCycleRoutine;


    protected override void AwakeSingleton()
    {
        PlatformGeneratorPresenter = gameObject.GetComponentInChildren<PlatformGeneratorPresenter>();
        ColorSchemePresenter = gameObject.GetComponentInChildren<ColorSchemePresenter>();
        FlashGeneratorPresenter = gameObject.GetComponentInChildren<FlashGeneratorPresenter>();

        if (lifeCycleRoutine == null) lifeCycleRoutine = StartCoroutine(LifeCycleEnumerator());
    }


    private IEnumerator LifeCycleEnumerator()
    {
        SetDefaultGenerationRules();

        while (true)
        {
            yield return new WaitForSeconds(timePeriodForTheGenerationRules);
            SetNewGenerationRules();
        }
    }


    private void SetDefaultGenerationRules()
    {
        PlatformGeneratorPresenter.SetDefaultPlatformGeneratorConfigs();
        ColorSchemePresenter.SetDefaultColorScheme();
    }


    private void SetNewGenerationRules()
    {
        PlatformGeneratorPresenter.SetNewPlatformGeneratorConfigs();
        ColorSchemePresenter.SetNewColorScheme();
    }
}
