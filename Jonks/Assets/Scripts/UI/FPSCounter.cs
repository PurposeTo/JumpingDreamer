using UnityEngine;
using TMPro;
using System;

public class FPSCounter : MonoBehaviour
{
    private TextMeshProUGUI FPSText;

    private void Start()
    {
        FPSText = gameObject.GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        double fps = Math.Round((double)(1.0 / Time.unscaledDeltaTime));
        FPSText.text = $"FPS: {fps}";
    }
}
