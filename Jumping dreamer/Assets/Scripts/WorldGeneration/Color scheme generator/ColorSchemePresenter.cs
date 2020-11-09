using UnityEngine;
using UnityEngine.UI;

public class ColorSchemePresenter : SuperMonoBehaviour
{
    [SerializeField] private Image backgroundImageToCamera = null;
    [SerializeField] private ColorSchemeData colorSchemeData = null;
    private ColorSchemeGenerator colorSchemeGenerator;

    protected override void AwakeWrapped()
    {
        colorSchemeGenerator = new ColorSchemeGenerator(this, colorSchemeData, backgroundImageToCamera);
        SetDefaultColorScheme();
    }


    public void SetNewColorScheme()
    {
        colorSchemeGenerator.SetRandomColorSchemeExcluding();
    }


    private void SetDefaultColorScheme()
    {
        colorSchemeGenerator.SetDefaultColorScheme();
    }
}
