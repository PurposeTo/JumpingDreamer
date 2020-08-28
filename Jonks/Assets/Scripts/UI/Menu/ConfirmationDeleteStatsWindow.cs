using System;
using UnityEngine;

public class ConfirmationDeleteStatsWindow : MonoBehaviour
{
    public void ConfirmDeleteStatsButton()
    {
        PlayerDataLocalStorageSafe.Instance.DeletePlayerData();
        CloseWindowBitton();
    }


    public void CloseWindowBitton()
    {
        gameObject.SetActive(false);
    }
}

