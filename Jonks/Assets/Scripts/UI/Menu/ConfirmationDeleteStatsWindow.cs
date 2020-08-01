using UnityEngine;
using System;

public class ConfirmationDeleteStatsWindow : MonoBehaviour
{
    public event EventHandler OnDeleteStats;


    public void ConfirmDeleteStatsButton()
    {
        OnDeleteStats?.Invoke(this, null);
        CloseWindowBitton();
    }


    public void CloseWindowBitton()
    {
        gameObject.SetActive(false);
    }
}

