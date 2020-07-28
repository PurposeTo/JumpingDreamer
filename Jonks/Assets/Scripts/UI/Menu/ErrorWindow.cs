using UnityEngine;
using TMPro;

public class ErrorWindow : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI errorTextObject;

    private Animator panelAnimator;


    private void Start()
    {
        panelAnimator = panel.GetComponent<Animator>();
    }


    public void CloseClickHandler()
    {
        //panelAnimator
        Destroy(gameObject);
    }
}
