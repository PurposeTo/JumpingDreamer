using UnityEngine;
using TMPro;

public class ErrorWindow : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI errorTextObject;

    private Animator panelAnimator;


    private void Start()
    {
        Shutter.Instance.OnLoadFileError += InitializeErrorWindow;
    }


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


    public void CloseClickHandler()
    {
        //panelAnimator
        Destroy(gameObject);
    }
}
