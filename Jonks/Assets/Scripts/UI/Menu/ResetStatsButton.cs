using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class ResetStatsButton : MonoBehaviour
{
    public ConfirmationDeleteStatsWindow ConfirmationDeleteWindow;
    private static bool isStatsAlreadyReset = false;


    private void Start()
    {
        ConfirmationDeleteWindow.OnDeleteStats += TurnOffButton;

        if (File.Exists(PlayerStatsDataStorageSafe.Instance.FilePath) && !isStatsAlreadyReset)
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
        ConfirmationDeleteWindow.OnDeleteStats -= TurnOffButton;
    }


    public void ResetPlayerStatsData()
    {
        ConfirmationDeleteWindow.gameObject.SetActive(true);
    }


    private void TurnOffButton(object sender, EventArgs eventArgs)
    {
        gameObject.GetComponent<Button>().interactable = false;
        isStatsAlreadyReset = true;
    }
}
