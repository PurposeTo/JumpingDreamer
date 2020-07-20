using UnityEngine;
using TMPro;
using Assets.Scripts.Player.Data;

public class ErrorWindow : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI errorTextObject;

    private Animator panelAnimator;


    private void InitializeErrorWindow(object sender, string errorText)
    {
        panelAnimator = panel.GetComponent<Animator>();
        errorTextObject.text = errorText;

        ShowErrorWindow();
    }


    private void ShowErrorWindow()
    {
        //panelAnimator
        gameObject.SetActive(true);
    }


    public void OKClickHandler()
    {
        //panelAnimator
        gameObject.SetActive(false);
    }
}
