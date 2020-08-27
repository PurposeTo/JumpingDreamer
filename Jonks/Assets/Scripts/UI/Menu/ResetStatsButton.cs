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
        PlayerDataLocalStorageSafe.Instance.OnDeleteStats += TurnOffButton;

        if (File.Exists(PlayerDataLocalStorageSafe.Instance.FilePath) && !PlayerDataLocalStorageSafe.IsPlayerDataAlreadyReset)
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
        PlayerDataLocalStorageSafe.Instance.OnDeleteStats -= TurnOffButton;
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
