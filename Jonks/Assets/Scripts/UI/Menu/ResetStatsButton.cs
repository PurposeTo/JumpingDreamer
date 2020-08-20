using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;
using GooglePlayGames.BasicApi.Multiplayer;

public class ResetStatsButton : MonoBehaviour
{
    public ConfirmationDeleteStatsWindow ConfirmationDeleteWindow;


    private void Start()
    {
        PlayerDataStorageSafe.Instance.OnDeleteStats += TurnOffButton;

        if (File.Exists(PlayerDataStorageSafe.Instance.FilePath) && !PlayerDataStorageSafe.IsPlayerDataAlreadyReset)
        {
            gameObject.GetComponent<Button>().interactable = true;
        }
        else
        {
            gameObject.GetComponent<Button>().interactable = false;
        }
    }


    private void OnDestroy()
    {
        PlayerDataStorageSafe.Instance.OnDeleteStats -= TurnOffButton;
    }


    public void ResetPlayerStatsData()
    {
        ConfirmationDeleteWindow.gameObject.SetActive(true);
    }


    private void TurnOffButton(object sender, EventArgs eventArgs)
    {
        gameObject.GetComponent<Button>().interactable = false;
    }
}
