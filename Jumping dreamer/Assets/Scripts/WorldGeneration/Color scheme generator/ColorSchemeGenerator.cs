using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorSchemeGenerator
{
    private readonly SuperMonoBehaviour superMonoBehaviour;
    private readonly ColorSchemeData colorSchemeData;
    private readonly Image backgroundImageToCamera;

    public ColorSchemeGenerator(SuperMonoBehaviour superMonoBehaviour, ColorSchemeData colorSchemeData, Image backgroundImageToCamera)
    {
        this.superMonoBehaviour = superMonoBehaviour != null ? superMonoBehaviour : throw new System.ArgumentNullException(nameof(superMonoBehaviour));
        this.colorSchemeData = colorSchemeData != null ? colorSchemeData : throw new System.ArgumentNullException(nameof(colorSchemeData));
        this.backgroundImageToCamera = backgroundImageToCamera != null ? backgroundImageToCamera : throw new System.ArgumentNullException(nameof(backgroundImageToCamera));

        changeColorSchemeInfo = superMonoBehaviour.CreateCoroutineInfo();
    }


    private readonly float timeToChangeScheme = 1f;

    private Color currentSetColorScheme;

    private ICoroutineInfo changeColorSchemeInfo;


    public void SetDefaultColorScheme()
    {
        ChangeColorScheme(colorSchemeData.GetDefaultColorScheme());
    }


    public void SetRandomColorSchemeExcluding()
    {
        ChangeColorScheme(colorSchemeData.GetRandomColorSchemeExcluding(currentSetColorScheme));
    }


    private void ChangeColorScheme(Color color)
    {
        currentSetColorScheme = color;

        superMonoBehaviour.ReStartCoroutineExecution(ref changeColorSchemeInfo, ChangeColorSchemeEnumerator(color));
    }


    private void ChangeColorByT(Color currentOldColor, Color newColor, float t)
    {
        backgroundImageToCamera.color = Color.Lerp(currentOldColor, newColor, t);
    }


    private IEnumerator ChangeColorSchemeEnumerator(Color colorToSet)
    {
        float counter = 0f;
        Color currentOldColor = backgroundImageToCamera.color;

        bool isColorChanged()
        {
            ChangeColorByT(currentOldColor, colorToSet, counter);
            counter += Time.deltaTime;
            return counter / timeToChangeScheme >= 1f;
        }

        yield return new WaitUntil(isColorChanged);
    }
}
