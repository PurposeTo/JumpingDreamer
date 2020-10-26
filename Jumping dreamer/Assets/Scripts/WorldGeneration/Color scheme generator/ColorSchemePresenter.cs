﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ColorSchemeGenerator))]
public class ColorSchemePresenter : MonoBehaviour
{
    [SerializeField] private Image backgroundImageToCamera = null;
    [SerializeField] private ColorSchemeData colorSchemeData = null;
    private ColorSchemeGenerator colorSchemeGenerator;

    private void Awake()
    {
        colorSchemeGenerator = gameObject.GetComponent<ColorSchemeGenerator>();
        colorSchemeGenerator.Constructor(colorSchemeData, backgroundImageToCamera);
    }


    public void SetDefaultColorScheme()
    {
        colorSchemeGenerator.SetDefaultColorScheme();
    }


    public void SetNewColorScheme()
    {
        colorSchemeGenerator.SetRandomColorSchemeExcluding();
    }
}
