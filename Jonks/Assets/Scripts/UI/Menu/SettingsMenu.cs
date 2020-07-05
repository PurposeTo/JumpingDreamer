using System.Collections;
using System.Collections.Generic;
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
        animator.SetTrigger("Open");
    }


    public void CloseSettingsMenu()
    {
        animator.SetTrigger("Close");
    }
}
