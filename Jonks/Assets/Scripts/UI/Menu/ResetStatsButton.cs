using UnityEngine;
using System.IO;
using Assets.Scripts.Player.Data;
using UnityEngine.UI;
using System;

public class ResetStatsButton : MonoBehaviour
{
    public ConfirmationDeleteStatsWindow confirmationDeleteWindow;


    private void Start()
    {
        if (File.Exists(PlayerStatsDataStorageSafe.Instance.FilePath))
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }


    public void ResetPlayerStatsData()
    {
        confirmationDeleteWindow.gameObject.SetActive(true);
    }
}
