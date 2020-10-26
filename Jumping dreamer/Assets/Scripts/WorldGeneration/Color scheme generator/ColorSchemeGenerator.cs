using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ColorSchemeGenerator
{
    public ColorSchemeGenerator(ColorSchemeData colorSchemeData, Image backgroundImageToCamera)
    {
        if (colorSchemeData == null) throw new System.ArgumentNullException("colorSchemeData");
        if (backgroundImageToCamera == null) throw new System.ArgumentNullException("backgroundImageToCamera");

        this.colorSchemeData = colorSchemeData;
        this.backgroundImageToCamera = backgroundImageToCamera;
    }

    private readonly float timeToChangeScheme = 1f;
    private ColorSchemeData colorSchemeData;
    private Image backgroundImageToCamera;

    public Coroutine ChangeColorSchemeRoutine;


    public IEnumerator ChangeColorSchemeEnumerator(Color colorToSet)
    {
        float counter = 0f;
        Color currentOldColor = backgroundImageToCamera.color;

        bool isColorChanged()
        {
            ChangeColorByT(currentOldColor, colorToSet, counter);
            counter += Time.deltaTime;
            return counter/ timeToChangeScheme >= 1f;
        }

        yield return new WaitUntil(isColorChanged);

        ChangeColorSchemeRoutine = null;
    }


    public Color GetDefaultColorScheme()
    {
        return colorSchemeData.GetDefaultColorScheme();
    }


    public Color GetRandomColorSchemeExcluding()
    {
        return colorSchemeData.GetRandomColorSchemeExcluding(backgroundImageToCamera.color);
    }


    private void ChangeColorByT(Color currentOldColor, Color newColor, float t)
    {
        backgroundImageToCamera.color = Color.Lerp(currentOldColor, newColor, t);
    }
}
