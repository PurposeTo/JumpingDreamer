using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorSchemeGenerator : MonoBehaviour
{
    private readonly float timeToChangeScheme = 1f;
    private ColorSchemeData colorSchemeData;
    private Image backgroundImageToCamera;

    private Color currentSetColorScheme;

    private CoroutineExecutor CoroutineExecutor => CoroutineExecutor.Instance;
    private ICoroutineInfo changeColorSchemeInfo;


    public void Constructor(ColorSchemeData colorSchemeData, Image backgroundImageToCamera)
    {
        if (colorSchemeData == null) throw new System.ArgumentNullException("colorSchemeData");
        if (backgroundImageToCamera == null) throw new System.ArgumentNullException("backgroundImageToCamera");

        this.colorSchemeData = colorSchemeData;
        this.backgroundImageToCamera = backgroundImageToCamera;

        CoroutineExecutor.SetCommandToQueue(() =>
       changeColorSchemeInfo = CoroutineExecutor.CreateCoroutineInfo());
    }


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

        CoroutineExecutor.ReStartCoroutineExecution(changeColorSchemeInfo, ChangeColorSchemeEnumerator(color));
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
