using UnityEngine;
using TMPro;

public class DialogWindow : MonoBehaviour
{
    public GameObject panel;
    public TextMeshProUGUI textMeshProToShow;

    private Animator panelAnimator;


    private void Start()
    {
        panelAnimator = panel.GetComponent<Animator>();
    }


    public void CloseClickHandler()
    {
        // TODO: Panel closing animation
        Destroy(gameObject);
    }
}
