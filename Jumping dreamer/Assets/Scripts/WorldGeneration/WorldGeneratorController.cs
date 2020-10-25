using System.Collections;
using UnityEngine;

public class WorldGeneratorController : SingletonMonoBehaviour<WorldGeneratorController>
{
    public PlatformGeneratorPresenter PlatformGeneratorPresenter { get; private set; }
    public ColorSchemePresenter ColorSchemePresenter { get; private set; }

    private readonly float timePeriodForTheGenerationRules = 30f;
    private Coroutine lifeCycleRoutine;


    protected override void AwakeSingleton()
    {
        PlatformGeneratorPresenter = gameObject.GetComponent<PlatformGeneratorPresenter>();
        ColorSchemePresenter = gameObject.GetComponent<ColorSchemePresenter>();
    }


    private void Start()
    {
        if (lifeCycleRoutine == null) lifeCycleRoutine = StartCoroutine(LifeCycleEnumerator());
    }


    private IEnumerator LifeCycleEnumerator()
    {
        SetDefaultGenerationRules();
        StartWorldInitialization();

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


    private void StartWorldInitialization()
    {
        PlatformGeneratorPresenter.StartPlatformGeneratorInitialization();
    }


    private void SetNewGenerationRules()
    {
        PlatformGeneratorPresenter.SetNewPlatformGeneratorConfigs();
        ColorSchemePresenter.SetNewColorScheme();
    }
}
