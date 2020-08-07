using System;
using UnityEngine;

public class ConfirmationDeleteStatsWindow : MonoBehaviour
{
    public event EventHandler OnDeleteStats;

    public void ConfirmDeleteStatsButton()
    {
        PlayerStatsDataStorageSafe.Instance.DeletePlayerStatsData();
        OnDeleteStats?.Invoke(this, null);
        CloseWindowBitton();
    }


    public void CloseWindowBitton()
    {
        gameObject.SetActive(false);
    }
}

