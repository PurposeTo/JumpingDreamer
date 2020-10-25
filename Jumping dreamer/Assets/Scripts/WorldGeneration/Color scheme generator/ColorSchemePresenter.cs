using UnityEngine;
using UnityEngine.UI;

public class ColorSchemePresenter : MonoBehaviour
{
    [SerializeField] private Image backgroundImageToCamera;
    [SerializeField] private ColorSchemeData colorSchemeData = null;
    private ColorSchemeGenerator colorSchemeGenerator;

    private void Awake()
    {
        colorSchemeGenerator = new ColorSchemeGenerator(colorSchemeData, backgroundImageToCamera);
    }


    public void SetDefaultColorScheme()
    {
        ChangeColorSchemeRoutine(colorSchemeGenerator.GetDefaultColorScheme());
    }


    public void SetNewColorScheme()
    {
        ChangeColorSchemeRoutine(colorSchemeGenerator.GetRandomColorSchemeExcluding());
    }


    private void ChangeColorSchemeRoutine(Color color)
    {
        if (colorSchemeGenerator.ChangeColorSchemeRoutine != null)
        {
            StopCoroutine(colorSchemeGenerator.ChangeColorSchemeRoutine);
            colorSchemeGenerator.ChangeColorSchemeRoutine = null;
        }

        colorSchemeGenerator.ChangeColorSchemeRoutine = StartCoroutine(colorSchemeGenerator.ChangeColorSchemeEnumerator(color));
    }
}
