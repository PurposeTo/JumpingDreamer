using UnityEngine;

public class SettingsMenu : MonoBehaviour
{
    public GameObject SettingsButton;
    public GameObject SettingsScreen;
    private Animator animator;


    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
    }


    public void OpenSettingsMenu()
    {
        animator.SetBool("isOpen", true);
    }


    public void CloseSettingsMenu()
    {
        animator.SetBool("isOpen", false);
    }
}
