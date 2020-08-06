using UnityEngine;
using System;
using UnityEngine.UI;

public class ConfirmationDeleteStatsWindow : MonoBehaviour
{
    public event EventHandler OnDeleteStats;


    public void ConfirmDeleteStatsButton()
    {
        OnDeleteStats?.Invoke(this, null);
        MainMenu.Instance.SettingsMenu.ResetStatsButton.GetComponent<Button>().interactable = false;
        CloseWindowBitton();
    }


    public void CloseWindowBitton()
    {
        gameObject.SetActive(false);
    }
}

