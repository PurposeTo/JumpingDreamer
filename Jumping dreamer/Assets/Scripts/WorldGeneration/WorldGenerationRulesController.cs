using System.Collections;
using UnityEngine;

public class WorldGenerationRulesController : SingletonSuperMonoBehaviour<WorldGenerationRulesController>
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
        while (true)
        {
            yield return new WaitForSeconds(timePeriodForTheGenerationRules);
            SetNewGenerationRules();
        }
    }


    // Сначала необходимо установить правила генерации для платформ, тк остальные правила могут быть зависимы от них
    private void SetNewGenerationRules()
    {
        PlatformGeneratorPresenter.SetNewPlatformGenerationConfigs();
        ColorSchemePresenter.SetNewColorScheme();
        FlashGeneratorPresenter.SetRandomFlashGenerationConfigs();
    }
}
