using UnityEngine;

public class ErrorWindowGenerator : SingletonMonoBehaviour<ErrorWindowGenerator>
{
    [SerializeField] private GameObject errorWindow = null;

    public void CreateErrorWindow(string errorText)
    {
        Instantiate(errorWindow);
        string errorMessage = errorText;
        errorWindow.GetComponent<ErrorWindow>().errorTextObject.text = errorMessage;
    }
}
