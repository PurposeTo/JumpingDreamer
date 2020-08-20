using System;
using UnityEngine;

public class ConfirmationDeleteStatsWindow : MonoBehaviour
{
    public void ConfirmDeleteStatsButton()
    {
        PlayerDataStorageSafe.Instance.DeletePlayerStatsData();
        CloseWindowBitton();
    }


    public void CloseWindowBitton()
    {
        gameObject.SetActive(false);
    }
}

